import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  forgotForm!: FormGroup;

  isLoading = false;
  isForgotLoading = false;
  showForgotModal = false;
  loginError = '';
  forgotError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(7)]]
    });

    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get emailControl() {
    return this.form.get('email')!;
  }

  get passwordControl() {
    return this.form.get('password')!;
  }

  get forgotEmailControl() {
    return this.forgotForm.get('email')!;
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

  getForgotEmailError(): string {
    const ctrl = this.forgotEmailControl;
    if (!ctrl.touched && !ctrl.dirty) return '';
    if (ctrl.hasError('required')) return 'Campo obrigatório';
    if (ctrl.hasError('email')) return 'Email inválido';
    return '';
  }

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.loginError = '';
    this.isLoading = true;

    this.authService
      .login({ email: this.emailControl.value, password: this.passwordControl.value })
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.toastService.success('Bem-vindo!');
          this.router.navigate(['/auth/login']);
        },
        error: err => {
          this.isLoading = false;
          if (err.status === 401) {
            this.loginError = 'Email ou senha inválidos';
          } else {
            this.loginError = 'Erro ao conectar. Tente novamente.';
          }
        }
      });
  }

  openForgotModal(): void {
    this.forgotForm.reset();
    this.forgotError = '';
    this.showForgotModal = true;
  }

  closeForgotModal(): void {
    this.showForgotModal = false;
  }

  onModalBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeForgotModal();
    }
  }

  submitForgot(): void {
    this.forgotForm.markAllAsTouched();
    if (this.forgotForm.invalid || this.isForgotLoading) {
      return;
    }

    this.forgotError = '';
    this.isForgotLoading = true;

    this.authService.forgotPassword(this.forgotEmailControl.value).subscribe({
      next: () => {
        this.isForgotLoading = false;
        this.toastService.success('Link enviado para seu email');
        this.closeForgotModal();
      },
      error: err => {
        this.isForgotLoading = false;
        if (err.status === 404) {
          this.forgotError = 'Email não encontrado';
        } else {
          this.forgotError = 'Erro ao conectar. Tente novamente.';
        }
      }
    });
  }
}
