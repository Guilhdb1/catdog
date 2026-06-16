import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginPayload {
  email: string;
  password: string;
}

export interface RegisterPayload {
  email: string;
  password: string;
}

export interface UserInfo {
  id: string;
  email: string;
  role: string;
}

export interface LoginResponse {
  token: string;
  user: UserInfo;
}

export interface RegisterResponse {
  user: { id: string; email: string };
  message: string;
}

export interface MessageResponse {
  message: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiBase = '/auth';
  private readonly tokenKey = 'token';

  constructor(private http: HttpClient) {}

  login(payload: LoginPayload): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiBase}/login`, payload)
      .pipe(tap(response => this.storeToken(response.token)));
  }

  register(payload: RegisterPayload): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiBase}/register`, payload);
  }

  forgotPassword(email: string): Observable<MessageResponse> {
    return this.http.post<MessageResponse>(`${this.apiBase}/forgot-password`, { email });
  }

  resetPassword(token: string, newPassword: string): Observable<MessageResponse> {
    return this.http.post<MessageResponse>(`${this.apiBase}/reset-password`, {
      token,
      newPassword
    });
  }

  confirmEmail(token: string): Observable<MessageResponse> {
    return this.http.get<MessageResponse>(`${this.apiBase}/confirm`, { params: { token } });
  }

  logout(): Observable<MessageResponse> {
    return this.http
      .post<MessageResponse>(`${this.apiBase}/logout`, {})
      .pipe(tap(() => this.clearToken()));
  }

  storeToken(token: string): void {
    sessionStorage.setItem(this.tokenKey, token);
  }

  clearToken(): void {
    sessionStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }
}
