import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  form: FormGroup;
  message = '';
  error = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submit() {
    if (this.form.invalid) {
      return;
    }

    this.error = '';
    this.message = '';
    this.authService.forgotPassword(this.form.value.email).subscribe({
      next: result => {
        this.message = result.message;
      },
      error: err => {
        this.error = err.error?.error || 'Erro ao enviar instruções de recuperação.';
      }
    });
  }
}
