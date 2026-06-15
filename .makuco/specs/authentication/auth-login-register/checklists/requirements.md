## Checklist de Validação da Spec — Requisitos de Autenticação
Data da validação: 2026-06-15

Itens de verificação:

1. Registro com confirmação por e‑mail criado e e‑mail enviado (24h) — Resultado: PASS
   - Evidência: `spec_context.md` seção "Fluxos de Alta Nível" e "Critérios de Aceitação" descrevem o `POST /auth/register`, geração de `confirmation_token` com 24h e envio do e‑mail.

2. Login retorna access + refresh tokens e role; redirecionamento por role definido — Resultado: PASS
   - Evidência: `spec_context.md` seção "Requisitos Funcionais" e "Fluxos de Alta Nível" listam `POST /auth/login`, tokens e redirecionamento para `/admin/dashboard` e `/adotante/home`.

3. Refresh token rotativo com invalidação do anterior e detecção de reuse — Resultado: PASS
   - Evidência: `spec_context.md` seção "Regras de Negócio e Segurança" e "Fluxos de Alta Nível" descrevem rotation, armazenamento de hash e replay handling.

4. Política de senha e hashing seguro documentados — Resultado: PASS
   - Evidência: `spec_context.md` seção "Regras de Negócio e Segurança" especifica política de complexidade e recomenda Argon2id/bcrypt.

5. Endpoints mínimos listados — Resultado: PASS
   - Evidência: `spec_context.md` seção "Endpoints Requeridos" contém a lista de endpoints necessários.

6. Recuperação de senha com token de 1h e invalidação de sessions após reset — Resultado: PASS
   - Evidência: `spec_context.md` seção "Fluxos de Alta Nível" e "Critérios de Aceitação".

7. Email template descrito (texto e layout) — Resultado: PASS
   - Evidência: `spec_context.md` seção "Template de E‑mail de Confirmação" contém subject, preheader, corpo e CTA.

8. Decisão de TTL de tokens consolidada (sem expiração) sem entradas conflitantes — Resultado: PASS
   - Evidência: `spec_context.md` contém apenas a versão "sem expiração" nas seções Objetivos, Regras de Negócio, Fluxos e Critérios de Aceitação, com nota de segurança e mitigações documentadas.

9. Critério de aceitação para `GET /auth/me` presente — Resultado: PASS
   - Evidência: `spec_context.md` seção "Critérios de Aceitação" inclui CA específico para o endpoint.

10. Perguntas abertas 1 (TTL), 3 (cookie) e 4 (rotas) encerradas — Resultado: PASS
    - Evidência: `spec_context.md` seção "Perguntas Abertas / Bloqueantes" marca as três questões como resolvidas com decisão documentada.

11. Pergunta 2 (provedor de e-mail transacional e branding) — Resultado: PENDENTE
    - **Bloqueante para implementação:** provedor de e-mail, domínio e assets de branding ainda não definidos. Implementar com placeholders até confirmação.

Conclusão da validação (ALT-001 — 2026-06-15): Itens 1–10 aprovados. Item 11 pendente e bloqueia implementação de e-mail e templates visuais.
