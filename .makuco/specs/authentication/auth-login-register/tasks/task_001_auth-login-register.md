# TASK-AUTH-001 — Authentication core: login/register, email confirmation, refresh rotation

**Root (backend)**: `src/Modules/Authentication/`
**Root (frontend)**: `src/app/auth/`
**Branch**: `feature/TASK-AUTH-001-auth-login-register`
**Spec**: `.makuco/specs/authentication/auth-login-register/spec_context.md`
**Part**: 1 of 1 — authentication core
**Generated**: 2026-06-15

## Context
Implement the CatDog authentication core with email confirmation, JWT access, rotative refresh tokens, logout, password reset, and role-based redirect. The auth UI should follow the provided centered card style for login/register screens. See spec for password policy, token rules, and redirect routes.

## Scope
**In:**
- Backend auth API for `POST /auth/register`, `GET /auth/confirm`, `POST /auth/login`, `POST /auth/refresh`, `POST /auth/logout`, `POST /auth/forgot`, `POST /auth/reset`, and `GET /auth/me`.
- Email confirmation token lifecycle and password reset token lifecycle.
- Refresh token rotation with hash persistence, reuse detection, revocation, and secure `HttpOnly SameSite` cookie delivery.
- Login result includes `role`, `user`, and frontend redirect guidance for `/admin/dashboard` or `/adotante/home`.
- Frontend auth pages for login, register, confirm email, forgot password, and reset password using a centered auth card layout.

**Out:**
- No MFA or social login.
- No admin panel screens beyond the redirect route decision.
- No unrelated user-management or domain modules.
- No visual redesign outside the auth page/card layout and required validation/error states.

## Ubiquitous Language
| Business Term | Code Mapping |
|---|---|
| access token | JWT access token, short-lived auth claim set |
| refresh token | secure cookie-backed token stored as DB hash |
| email confirmation | `GET /auth/confirm?token=...` flow |
| password reset | `POST /auth/reset` flow with one-hour token |
| role redirect | redirect destination based on `role` after login |

## Files
| Action | Path | Why |
|---|---|---|
| create | `src/Modules/Authentication/Entities/User.cs` | entidade `users`: id, nome, email, password_hash, email_confirmed, email_confirmed_at, roles, failed_login_attempts, locked_until, created_at, updated_at |
| create | `src/Modules/Authentication/Entities/RefreshToken.cs` | entidade `refresh_tokens`: id, user_id, token_hash, issued_at, expires_at, revoked, revoked_at, last_used_at, device_info |
| create | `src/Modules/Authentication/Entities/ConfirmationToken.cs` | entidade `confirmation_tokens`: user_id, token_hash, expires_at, used_at |
| create | `src/Modules/Authentication/Entities/PasswordResetToken.cs` | entidade `password_reset_tokens`: user_id, token_hash, expires_at, used_at |
| create | `src/Modules/Authentication/DTOs/RegisterRequest.cs` | payload de entrada para POST /auth/register |
| create | `src/Modules/Authentication/DTOs/LoginRequest.cs` | payload de entrada para POST /auth/login |
| create | `src/Modules/Authentication/DTOs/LoginResponse.cs` | resposta de login: access_token, user \{ id, nome, email \}, role, redirectTo |
| create | `src/Modules/Authentication/DTOs/ForgotPasswordRequest.cs` | payload para POST /auth/forgot |
| create | `src/Modules/Authentication/DTOs/ResetPasswordRequest.cs` | payload para POST /auth/reset |
| create | `src/Modules/Authentication/DTOs/MeResponse.cs` | resposta para GET /auth/me: id, nome, email, role |
| create | `src/Modules/Authentication/Repositories/IAuthRepository.cs` | interface de acesso a dados de autenticação |
| create | `src/Modules/Authentication/Repositories/AuthRepository.cs` | implementação EF Core do repositório de autenticação |
| create | `src/Modules/Authentication/Services/EmailService.cs` | envio de e-mails de confirmação (subject: "Confirme sua conta no CatDog") e reset de senha |
| create | `src/Migrations/CreateAuthTables.cs` | migration que cria as tabelas users, refresh_tokens, confirmation_tokens e password_reset_tokens |
| create | `src/Modules/Authentication/Controllers/AuthController.cs` | auth API entrypoints |
| create | `src/Modules/Authentication/Services/AuthService.cs` | auth business rules |
| create | `src/Modules/Authentication/Infrastructure/TokenManager.cs` | JWT + refresh logic |
| create | `src/app/auth/auth.module.ts` | frontend auth feature module e registro de rotas |
| create | `src/app/auth/services/auth.service.ts` | serviço Angular que consome os endpoints de autenticação e expõe redirectTo e role |
| create | `src/app/auth/components/login/login.component.ts` | página de login com card centralizado |
| create | `src/app/auth/components/register/register.component.ts` | página de registro com card centralizado |
| create | `src/app/auth/components/confirm-email/confirm-email.component.ts` | página de confirmação de e-mail |
| create | `src/app/auth/components/forgot-password/forgot-password.component.ts` | página de recuperação de senha |
| create | `src/app/auth/components/reset-password/reset-password.component.ts` | página de redefinição de senha |

## Implementation

### `src/Modules/Authentication/Controllers/AuthController.cs` *(create)*
**Novo arquivo** (projeto greenfield — sem arquivo de referência existente no codebase).
**O que implementar**:
- Add endpoints: `/auth/register`, `/auth/confirm`, `/auth/login`, `/auth/refresh`, `/auth/logout`, `/auth/forgot`, `/auth/reset`, `/auth/me`.
- Return `role`, minimal `user` payload, and `redirectTo` after login.
- Set refresh cookie using `HttpOnly`, `Secure`, `SameSite=Strict` and clear it on logout.
- Return 403/401 when login is attempted before email confirmation or when account is locked.

### `src/Modules/Authentication/Services/AuthService.cs` *(create)*
**Novo arquivo** (projeto greenfield — sem arquivo de referência existente no codebase).
**O que implementar**:
- Aplicar política de senha: mínimo 10 caracteres, ao menos 1 maiúscula, 1 minúscula, 1 número e 1 caractere especial; rejeitar se a senha contiver o `nome` ou `email` do usuário como substring (comparação case-insensitive); bloquear senhas triviais (ex.: `123456789`, `password123`, `qwerty`).
- Track failed login attempts and set `locked_until` for 15 minutes after 5 consecutive failed attempts.
- Generate email confirmation tokens valid 24h and password reset tokens valid 1h.
- Implement refresh rotation: new refresh token issued, previous refresh marked revoked, reuse detection revokes all user sessions.
- Invalidate all active refresh tokens after password reset.

### `src/Modules/Authentication/Infrastructure/TokenManager.cs` *(create)*
**Novo arquivo** (projeto greenfield — sem arquivo de referência existente no codebase).
**O que implementar**:
- Create access JWT without expiry (decisão de negócio confirmada — ver spec) and claims `sub`, `email`, `role`, `name`, `iat`, `jti`.
- Generate cryptographically strong refresh token strings and persist only their hash.
- Expose method to build secure refresh-cookie metadata for `/auth/login` and `/auth/refresh`.
- Support token reuse detection and revocation reason tracking.

### `src/app/auth/auth.module.ts` *(create)*
**Novo arquivo** (projeto greenfield — sem arquivo de referência existente no codebase).
**O que implementar**:
- Register auth routes for `/auth/login`, `/auth/register`, `/auth/confirm-email`, `/auth/forgot-password`, `/auth/reset-password`.
- Provide `AuthService` and shared centered card layout for all auth pages.
- Add components: `LoginComponent`, `RegisterComponent`, `ConfirmEmailComponent`, `ForgotPasswordComponent`, `ResetPasswordComponent`.
- The auth pages must render a centered form card (max-width 420px, padding 32px, fundo branco, sombra suave sobre page background cinza claro) and show success/error states for each flow. **Pendência:** branding assets (logo, cores, Figma link) aguardam definição do provedor de e-mail e identidade visual (ver spec Q2 em aberto) — implementar com placeholders até confirmação.
- `AuthService` should call backend endpoints and surface `redirectTo` plus `role`.

## Acceptance Criteria
- [ ] **Given** a user submits `POST /auth/register` with valid `nome`, `email`, `senha`, and `confirmacao_senha`, **When** data is valid, **Then** create a non-confirmed user, send a confirmation email, and return a generic success response.
- [ ] **Given** a valid email confirmation token, **When** `GET /auth/confirm?token=...` is called, **Then** mark `email_confirmed=true`, set `email_confirmed_at`, and return success.
- [ ] **Given** an email-confirmed user submits correct credentials to `POST /auth/login`, **When** authentication succeeds, **Then** return `access_token`, set refresh token cookie, return `user`, `role`, and correct `redirectTo` route.
- [ ] **Given** a user submits wrong password repeatedly, **When** the 5th failure occurs, **Then** lock the account for 15 minutes and further logins return lockout response.
- [ ] **Given** a valid refresh cookie is sent to `POST /auth/refresh`, **When** the previous refresh token is active, **Then** issue a new access token, issue a new refresh cookie, and revoke the prior refresh token.
- [ ] **Given** a revoked refresh token is reused, **When** it is presented to `/auth/refresh`, **Then** revoke all refresh sessions for that user and force re-login.
- [ ] **Given** `POST /auth/logout` with a current refresh cookie, **When** logout succeeds, **Then** revoke that refresh token and clear the refresh cookie.
- [ ] **Given** `POST /auth/forgot` with any email, **When** request is processed, **Then** return a generic success response without revealing account existence and send a reset token email if the user exists.
- [ ] **Given** a valid reset token and matching new password, **When** `POST /auth/reset` completes, **Then** update the password, invalidate all active refresh tokens, and return success.
- [ ] **Given** an unconfirmed user attempts login, **When** credentials are valid, **Then** deny authentication with a confirmation-required error.
- [ ] **Given** login succeeds for `ADMIN`, **When** front-end consumes the response, **Then** it redirects to `/admin/dashboard`; for `ADOTANTE`, it redirects to `/adotante/home`.
- [ ] Auth pages render in a centered card layout and display form validation errors, success notifications, and API error messages.

## Authorization
- `ADMIN | ADOTANTE` → can authenticate and receive role-based redirect responses.
- `email_confirmed=false` → cannot complete login until confirmation is complete.

## API Notes
- **Endpoint**: `POST /auth/register`
  - Input: `{ nome, email, senha, confirmacao_senha }`
  - Success: `201` with generic message
  - Errors: `400` invalid payload; `409` email exists.
- **Endpoint**: `GET /auth/confirm?token=`
  - Success: `200` message
  - Errors: `400` invalid/expired token.
- **Endpoint**: `POST /auth/login`
  - Input: `{ email, senha }`
  - Success: `200` `{ access_token, user, role, redirectTo }` plus refresh cookie
  - Errors: `400` invalid credentials; `403` unconfirmed email; `423` locked account.
- **Endpoint**: `POST /auth/refresh`
  - Input: refresh cookie only
  - Success: `200` new access token, new refresh cookie
  - Errors: `401` missing/invalid token; `403` token reuse.
- **Endpoint**: `POST /auth/logout`
  - Input: refresh cookie only
  - Success: `200` message, cookie cleared
- **Endpoint**: `POST /auth/forgot`
  - Input: `{ email }`
  - Success: `200` generic message
- **Endpoint**: `POST /auth/reset`
  - Input: `{ token, senha, confirmacao_senha }`
  - Success: `200` message
  - Errors: `400` invalid/expired token.
- **Endpoint**: `GET /auth/me`
  - Input: `Authorization: Bearer <access_token>` (header)
  - Success: `200` `{ id, nome, email, role }`
  - Errors: `401` token ausente ou inválido; `403` token expirado.

## Dependencies
- **Requires**: none
