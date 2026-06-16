import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { passwordMatchValidator } from '../../validators/password-match.validator';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  form!: FormGroup;
  isLoading = false;
  resetError = '';
  token = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token') || '';

    this.form = this.fb.group(
      {
        password: ['', [Validators.required, Validators.minLength(7)]],
        confirmPassword: ['', [Validators.required]]
      },
      { validators: passwordMatchValidator('password', 'confirmPassword') }
    );
  }

  get passwordControl() {
    return this.form.get('password')!;
  }

  get confirmPasswordControl() {
    return this.form.get('confirmPassword')!;
  }

  getPasswordError(): string {
    const ctrl = this.passwordControl;
    if (!ctrl.touched && !ctrl.dirty) return '';
    if (ctrl.hasError('required')) return 'Campo obrigatório';
    if (ctrl.hasError('minlength')) return 'Senha deve ter no mínimo 7 caracteres';
    return '';
  }

  getConfirmPasswordError(): string {
    const ctrl = this.confirmPasswordControl;
    if (!ctrl.touched && !ctrl.dirty) return '';
    if (ctrl.hasError('required')) return 'Campo obrigatório';
    if (ctrl.hasError('passwordMismatch')) return 'Senhas não conferem';
    return '';
  }

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isLoading) {
      return;
    }

    if (!this.token) {
      this.resetError = 'Link inválido ou expirado. Solicite um novo.';
      return;
    }

    this.resetError = '';
    this.isLoading = true;

    this.authService.resetPassword(this.token, this.passwordControl.value).subscribe({
      next: () => {
        this.isLoading = false;
        this.toastService.success('Senha redefinida!');
        this.router.navigate(['/auth/login']);
      },
      error: err => {
        this.isLoading = false;
        if (err.status === 400 || err.status === 401) {
          this.resetError = 'Link inválido ou expirado. Solicite um novo.';
        } else {
          this.resetError = 'Erro ao conectar. Tente novamente.';
        }
      }
    });
  }
}
