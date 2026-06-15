# RF-001 Autenticação por Login e Senha com Confirmação por E-mail e Permissions

## Resumo Executivo
- Objetivo: Implementar autenticação por registro/login com confirmação por e-mail, sistema de permissões por ROLE (ADMIN / ADOTANTE), redirecionamento e layouts diferentes por role, e gestão segura de tokens com refresh rotativo.
- Contexto: Feature inicial de segurança/autenticação para a plataforma CatDog, necessária para controlar acesso, separar painéis e permitir operações administrativas seguras.
- Valor de negócio: Minimiza contas falsas, oferece controle de acesso por função, melhora a experiência do usuário por redirecionamento e layout por role, e aumenta a segurança com refresh token rotation e confirmação por e‑mail.

## Escopo
- Inclusões: registro com confirmação de e‑mail; login com access/refresh tokens; refresh rotativo; recuperação de senha; roles `ADMIN` e `ADOTANTE`; redirecionamento e layout por role; políticas de segurança (senha, bloqueio, hashing).
- Exclusões (primeira entrega): MFA, login social, painéis administrativos avançados (além do redirecionamento inicial), relatórios de sessões, gestão de roles (endpoint de mudança de role por ADMIN é escopo de feature administrativa futura).

## Objetivos e Critérios de Sucesso (Sucesso mensurável)
- Usuários podem criar conta e confirmar e‑mail em até 24h. (Métrica: 100% das confirmações válidas aceitas)
- Usuário autenticado recebe `access_token` e `refresh_token` sem limite de expiração (decisão de negócio confirmada pelo cliente — ver Regras de Negócio e mitigações). (Métrica: refresh rotation e reuse detection operacionais em 100% dos flows)
- Redirecionamento por role correto em 100% dos logins (rotas: `/admin/dashboard`, `/adotante/home`).
- Recuperação de senha envia link válido por 1h.

## Personas / Papéis
- Visitante
- Adotante (`ADOTANTE`) — role padrão
- Administrador (`ADMIN`)
- Serviço de e‑mail transacional
- Equipe de suporte/DevOps

## Requisitos Funcionais (Resumo)
1. Registro de usuário com `nome`, `email`, `senha`, `confirmacao_senha` (POST `/auth/register`). Conta criada como não confirmada; envia e‑mail de confirmação.
2. Confirmação de e‑mail via token (GET `/auth/confirm?token=...`) marcando `email_confirmed=true`.
3. Login (POST `/auth/login`) retorna `access_token`, `refresh_token`, `role`, e dados mínimos do usuário; bloqueio após N tentativas.
4. Refresh rotativo (POST `/auth/refresh`) que emite novo par access/refresh e invalida o refresh anterior.
5. Logout (POST `/auth/logout`) revoga refresh token atual.
6. Recuperação de senha (POST `/auth/forgot` e POST `/auth/reset`) com token expira em 1h.

## Regras de Negócio e Segurança
- Confirmação por e‑mail obrigatória antes de permitir ações autenticadas.
- Política de senha: mínimo 10 chars, 1 maiúscula, 1 minúscula, 1 número, 1 caractere especial; bloquear senhas triviais e similares ao nome ou e‑mail.
- Hash de senha com algoritmo moderno (ex.: Argon2id / bcrypt configurado).
- Access token: JWT sem limite de expiração (decisão de negócio confirmada pelo cliente).
- Refresh token: rotativo, sem limite de expiração; armazenar somente hash no DB; detectar replay e revogar todas as sessions do usuário em caso de reuse.
- Armazenamento cliente: HttpOnly Secure SameSite=Strict cookie para refresh token (decisão confirmada); access token em memória (JavaScript).
- **Nota de segurança (tokens sem expiração):** decisão explícita do cliente. Mitigações obrigatórias: (1) refresh rotation com reuse detection, (2) revogação total de sessions em caso de reuse detectado, (3) logout revoga refresh imediatamente, (4) reset de senha invalida todos os refresh tokens ativos, (5) auditoria de eventos de token via logs (sem logar tokens em texto claro).
- Rate limiting e bloqueio após 5 falhas de login por conta/IP com bloqueio de 15 minutos.
- Logs e auditoria: registrar created_at, last_login_at, failed_logins, eventos de token; não logar tokens em texto.
- Roles fixos: `ADMIN`, `ADOTANTE`. Mudança de role somente por `ADMIN` — endpoint de alteração de role é escopo de feature administrativa futura (ver Exclusões).

## Fluxos de Alta Nível
- Cadastro + confirmação por e‑mail
  1. `POST /auth/register` cria usuário (`email_confirmed=false`) e gera `confirmation_token` (24h).
  2. Envio de e‑mail com link de confirmação.
  3. `GET /auth/confirm?token=...` valida token e marca `email_confirmed=true`.
- Login
  1. `POST /auth/login` valida credenciais e `email_confirmed`.
  2. Emite `access_token` e `refresh_token` (sem expiração), grava hash do refresh em `refresh_tokens`.
  3. Retorna role; frontend redireciona por role.
- Refresh rotativo
  1. `POST /auth/refresh` valida refresh token, emite novo pair, invalida anterior.
  2. Em caso de reuse detectado, revogar todas sessions do usuário.
- Recuperação de senha
  1. `POST /auth/forgot` gera reset token (1h), envia e‑mail genérico.
  2. `POST /auth/reset` com token atualiza senha e invalida refresh tokens ativos.

## Modelo de Dados (mínimo)
- `users` (id: uuid, nome, email, password_hash, email_confirmed, email_confirmed_at, role: enum['ADMIN','ADOTANTE'], failed_login_attempts, locked_until, created_at, updated_at)
- `refresh_tokens` (id, user_id, token_hash, issued_at, expires_at, revoked, revoked_at, last_used_at, device_info)
- `confirmation_tokens` / `password_reset_tokens` (user_id, token_hash, expires_at, used_at)

## Endpoints Requeridos (API surface)
- `POST /auth/register`
- `GET /auth/confirm?token=`
- `POST /auth/login`
- `POST /auth/refresh`
- `POST /auth/logout`
- `POST /auth/forgot`
- `POST /auth/reset`
- `GET /auth/me`

## Exemplo de Payloads (resumo)
- `POST /auth/register` body: { "nome", "email", "senha", "confirmacao_senha" }
- `POST /auth/login` body: { "email", "senha" } → response: { "access_token", "refresh_token", "role", "user": { id,nome,email } }
- `POST /auth/refresh` body/cookie: { "refresh_token" } → response: new pair

## Template de E‑mail de Confirmação (texto e layout)
- Subject: "Confirme sua conta no CatDog — Ação necessária"
- Preheader: "Ative sua conta clicando no botão dentro de 24 horas."
- Conteúdo:
  - Header com logo CatDog
  - Título: "Bem‑vindo ao CatDog, [Nome]!"
  - Parágrafo: "Obrigado por criar sua conta. Para completar o registro e começar a usar a plataforma, confirme seu e‑mail clicando no botão abaixo. O link expira em 24 horas."
  - Botão CTA central: "Confirmar Conta"
  - Texto fallback com link completo
  - Observação de segurança e footer com links Terms/Privacy

## Não‑funcionais e Operacionais
- TLS obrigatório para todas comunicações.
- Chaves JWT/Secrets em secrets manager.
- Monitoramento de entregas de e‑mail (bounces) e logs de segurança.

## Riscos e Mitigações
- Replay de refresh tokens (ALTO) — mitigação: rotation + detectar reuse.
- Entrega de e‑mail (MED) — mitigação: configurar SPF/DKIM/DMARC e provider confiável.
- XSS/armazenamento inseguro (ALTO) — mitigação: HttpOnly cookies e orientação ao frontend.

## Assunções
- Domain, branding e provider de e‑mail serão fornecidos (logo, colors).
- URLs de redirect: `/admin/dashboard` e `/adotante/home` (configuráveis).
- Não haverá MFA no primeiro release.

## Perguntas Abertas / Bloqueantes
1. ~~TTLs desejados para `access_token` e `refresh_token`?~~ **Resolvida:** sem limite de expiração (decisão de negócio confirmada pelo cliente; mitigações documentadas nas Regras de Negócio).
2. Qual provedor de e‑mail transacional será usado e qual o domínio/branding exato? **Em aberto — bloqueia implementação de e‑mail e templates visuais. Fornecer antes de iniciar a feature.**
3. ~~Preferência de armazenamento de refresh token no cliente: HttpOnly cookie vs corpo de resposta?~~ **Resolvida:** HttpOnly Secure SameSite=Strict cookie (decisão confirmada).
4. ~~Rotas de redirecionamento definitivas após login/confirm/reset para cada role?~~ **Resolvidas:** ADMIN → `/admin/dashboard`; ADOTANTE → `/adotante/home`.

## Critérios de Aceitação (resumo)
- Registro cria conta não confirmada e envia e‑mail com token (24h).
- Login retorna access (sem expiração) + refresh rotativo (sem expiração) e role; frontend redireciona por role (`/admin/dashboard` e `/adotante/home`).
- Refresh rotativo emite novo par e invalida token anterior; reuse detectado revoga todas as sessions do usuário.
- Recuperação de senha envia token (1h) e invalida todos os refresh tokens ativos após reset.
- Política de senha aplicada; bloqueio após 5 tentativas por 15 minutos.
- `GET /auth/me` com token válido retorna `{ id, nome, email, role }`; token ausente ou inválido retorna 401.

---
JSON resumo (uso automático):
{
  "featureName": "Autenticação por Login e Senha com Confirmação por E‑mail e Permissions",
  "featureShortName": "Auth+Roles",
  "roles": ["ADMIN","ADOTANTE"],
  "requiredEndpoints": ["/auth/register","/auth/confirm","/auth/login","/auth/refresh","/auth/logout","/auth/forgot","/auth/reset","/auth/me"],
  "acceptanceCriteriaSummary": [
    "Registro cria conta não confirmada e envia e‑mail com token (24h).",
    "Login retorna access (sem expiração) + refresh (rotativo, sem expiração) e role; redireciona por role.",
    "Refresh rotativo emite novo par e invalida token anterior; reuse detectado revoga todas as sessions.",
    "Recuperação de senha envia token (1h) e invalida todos os refresh tokens ativos após reset.",
    "Password complexity enforce e conta bloqueada após 5 falhas por 15 minutos.",
    "GET /auth/me com token válido retorna id, nome, email, role; token ausente retorna 401."
  ]
}
