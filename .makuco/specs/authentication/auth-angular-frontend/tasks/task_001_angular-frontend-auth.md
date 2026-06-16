# TASK-AF-001 — Angular Frontend Authentication (FEATURE-002)

**Root (frontend)**: `src/app/auth/`
**Branch**: `feature/TASK-AF-001-angular-frontend-auth`
**Spec**: `.makuco/specs/authentication/auth-angular-frontend/spec_context.md`
**Part**: 1 of 1 — Angular auth UI
**Generated**: 2026-06-16

## Context
Implement the CatDog Angular 16 authentication frontend per FEATURE-002 spec. Rewrite existing auth components to correct field names, add real-time validation, toast notifications, forgot-password modal, sessionStorage token storage, and proper routing. No Angular Material (not in package.json), plain CSS only.

## Scope
**In:**
- Rewrite `AuthService` with correct API contracts (`password` not `senha`, correct endpoints)
- Create `ToastService` and `ToastComponent` in shared module
- Rewrite `LoginComponent` with forgot-password modal, loading state, toast, router navigation
- Rewrite `RegisterComponent` with password match validator, min 7 chars, toast, redirect to login
- Rewrite `ResetPasswordComponent` with correct field names, proper error messages, redirect
- Update `auth.module.ts` to remove forgot-password route, add SharedModule
- Update `AppModule` with `RouterModule.forRoot` and default redirect
- Style all components with CatDog branding, centered card layout, purple buttons

**Out:**
- Route guards
- Backend changes
- Angular Material components
- Remember Me functionality

## Files
| Action | Path | Why |
|---|---|---|
| create | `src/app/shared/services/toast.service.ts` | Toast notification service |
| create | `src/app/shared/components/toast/toast.component.ts` | Toast UI component |
| create | `src/app/shared/components/toast/toast.component.html` | Toast template |
| create | `src/app/shared/components/toast/toast.component.css` | Toast styles |
| create | `src/app/shared/shared.module.ts` | Shared module exports |
| update | `src/app/auth/services/auth.service.ts` | Fix field names and endpoints |
| update | `src/app/auth/components/login/login.component.ts` | Add modal, loading, toast |
| update | `src/app/auth/components/login/login.component.html` | Add modal template |
| update | `src/app/auth/components/login/login.component.css` | Style improvements |
| update | `src/app/auth/components/register/register.component.ts` | Fix validators, redirect |
| update | `src/app/auth/components/register/register.component.html` | Fix template |
| update | `src/app/auth/components/register/register.component.css` | Style improvements |
| update | `src/app/auth/components/reset-password/reset-password.component.ts` | Fix field names |
| update | `src/app/auth/components/reset-password/reset-password.component.html` | Fix template |
| update | `src/app/auth/components/reset-password/reset-password.component.css` | Style improvements |
| update | `src/app/auth/auth.module.ts` | Remove forgot-password route, add shared |
| update | `src/app/app.module.ts` | Add RouterModule.forRoot |
| update | `src/app/app.component.ts` | Add router-outlet and app-toast |

## Acceptance Criteria
- [ ] Login page renders with email, password fields, forgot-password link, register link
- [ ] Forgot-password opens as a modal overlay on login page (not a route)
- [ ] Real-time validation: email requires @, password min 7 chars, confirm password must match
- [ ] Buttons disabled during HTTP calls with loading indicator
- [ ] Toast messages shown on success (top-right corner)
- [ ] JWT token stored in sessionStorage after login
- [ ] Register redirects to /auth/login after success
- [ ] Reset password reads token from URL query param and redirects to login after success
- [ ] Purple buttons (#6B4EFF), centered card layout, CatDog branding
- [ ] Responsive design works at 320px, 768px, 1024px
