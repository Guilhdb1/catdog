RF-001 Autenticação por Login e Senha com Confirmação por E-mail

## Versão atual da spec
**Versão:** v1.0
**Spec original aprovada em:** 2026-06-15
**Última alteração:** 2026-06-15

## Histórico de Alterações

### ALT-001 — 2026-06-15
- Consolidada decisão de tokens sem limite de expiração (removidos registros conflitantes da versão anterior com TTL de 15min/30d)
- Adicionada nota de segurança com mitigações obrigatórias para tokens sem expiração
- Encerradas perguntas abertas 1 (TTL), 3 (cookie HttpOnly) e 4 (rotas de redirect)
- Adicionado critério de aceitação para `GET /auth/me`
- Corrigido modelo de dados: campo `roles` → `role: enum['ADMIN','ADOTANTE']` na entidade `users`
- Adicionada gestão de roles às exclusões (escopo de feature administrativa futura)
- Atualizado checklist de requisitos com novos itens de validação
