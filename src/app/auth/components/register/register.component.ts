import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { passwordMatchValidator } from '../../validators/password-match.validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  isLoading = false;
  registerError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(7)]],
        confirmPassword: ['', [Validators.required]]
      },
      { validators: passwordMatchValidator('password', 'confirmPassword') }
    );
  }

  get emailControl() {
    return this.form.get('email')!;
  }

  get passwordControl() {
    return this.form.get('password')!;
  }

  get confirmPasswordControl() {
    return this.form.get('confirmPassword')!;
  }

  getEmailError(): string {
    const ctrl = this.emailControl;
    if (!ctrl.touched && !ctrl.dirty) return '';
    if (ctrl.hasError('required')) return 'Campo obrigatório';
    if (ctrl.hasError('email')) return 'Email inválido';
    return '';
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

    this.registerError = '';
    this.isLoading = true;

    this.authService
      .register({ email: this.emailControl.value, password: this.passwordControl.value })
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.toastService.success('Conta criada! Faça login.');
          this.router.navigate(['/auth/login']);
        },
        error: err => {
          this.isLoading = false;
          if (err.status === 409) {
            this.registerError = 'Email já cadastrado';
          } else {
            this.registerError = 'Erro ao conectar. Tente novamente.';
          }
        }
      });
  }
}
