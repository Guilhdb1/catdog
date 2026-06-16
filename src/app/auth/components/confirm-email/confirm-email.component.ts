import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  message = '';
  error = '';

  constructor(private route: ActivatedRoute, private authService: AuthService) {}

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (!token) {
      this.error = 'Token de confirmação ausente.';
      return;
    }

    this.authService.confirmEmail(token).subscribe({
      next: result => {
        this.message = result.message;
      },
      error: err => {
        this.error = err.error?.error || 'Erro ao confirmar o e-mail.';
      }
    });
  }
}
