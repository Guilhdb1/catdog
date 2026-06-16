# FEATURE-002 — Angular Frontend para Autenticação

---

## Grupo 1 — Identificação

**Feature:** FEATURE-002 — Angular Frontend para Autenticação
**Módulo:** MODULE-001 — Autenticação
**Status:** Aprovado
**Criado por:** GitHub Copilot — 2026-06-16
**Aprovado por:** _A preencher_

---

## Objetivo da Feature

Construir uma interface Angular completa e responsiva para o sistema de autenticação CatDog, permitindo que novos usuários se registrem, realizem login com validação em tempo real, confirmem seus emails, redefinam senhas e gerenciem suas sessões de forma segura. A interface utiliza tokens JWT armazenados em sessionStorage, integra-se com a API REST já implementada e oferece feedback imediato ao usuário durante preenchimento de formulários.

---

## Grupo 2 — Contexto

### Quem Acessa

| Perfil / Permissão | Nível de acesso | Observação |
|---|---|---|
| Visitante não autenticado | Total | Acesso a login, registro, recuperação de senha |
| Usuário autenticado | Leitura/Escrita | Acesso a logout e dados de sessão |

---

### Premissas

- A API REST do backend (FEATURE-001) está implementada e funcional.
- Os usuários acessarão a aplicação via navegador moderno (Chrome, Firefox, Safari, Edge).
- A confirmação de email é automática no backend após registro; não há página de confirmação necessária no frontend.
- Os tokens JWT são armazenados em sessionStorage (não em localStorage) para maior segurança.
- A aplicação suporta português como idioma principal.
- Componentes Angular Material estarão disponíveis.

---

### Dependências

| Dependência | Tipo | Status | Impacto se não resolvida |
|---|---|---|---|
| FEATURE-001 — API Backend de Autenticação | FEATURE-ID | Resolvida | Bloqueia todos os fluxos de integração |
| Angular 16+ | Framework | Resolvida | Bloqueia implementação do frontend |
| Angular Material | Biblioteca | Resolvida | Afeta layout e componentes de UI |
| Node.js e npm | Tooling | Resolvida | Bloqueia build e execução |

---

### Referências e Insumos

**Protótipo / Wireframe:**
- Fornecido pelo usuário: imagem de login com CatDog branding, card-based centered layout, botão púrpura, campos email/senha, links para registro e recuperação de senha.

**Artefatos consultados:**
- FEATURE-001 spec_context.md — definições de endpoints, tokens, fluxos de autenticação.
- Product scope — confirmação automática de email, logout transparente.

**APIs consumidas:**
- POST `/auth/register` — registra novo usuário
- POST `/auth/login` — realiza login
- POST `/auth/logout` — limpa sessão (opcional)
- POST `/auth/forgot-password` — solicita reset de senha
- POST `/auth/reset-password` — redefine senha com token
- GET `/auth/me` — retorna dados do usuário autenticado (não usado nesta fase)

---

## Grupo 3 — Comportamento

### Histórias de Usuário

---

#### HU-001 — Usuário realiza login com email e senha

Usuário não autenticado acessa a página de login, preenche seu email registrado e senha correta, clica em "Entrar" e é autenticado no sistema.

**Pode ser testada independentemente:** Sim. A página de login é acessível sem autenticação prévia.

**Cenários de aceite:**

1. **Dado** que estou na página de login, **quando** digito um email válido (contém @) e uma senha com mais de 6 caracteres e clico "Entrar", **então** vejo uma notificação de sucesso e sou autenticado (token armazenado em sessionStorage).

2. **Dado** que estou na página de login, **quando** digito um email sem "@", **então** vejo uma mensagem de erro inline abaixo do campo email em tempo real.

3. **Dado** que estou na página de login, **quando** digito uma senha com menos de 7 caracteres, **então** vejo uma mensagem de erro inline abaixo do campo senha em tempo real.

4. **Dado** que estou na página de login, **quando** digito um email válido mas senha incorreta, **então** vejo uma mensagem de erro "Email ou senha inválidos".

---

#### HU-002 — Novo usuário se registra

Visitante acessa a página de registro, fornece email único, cria uma senha e confirmação, valida os dados em tempo real e clica para registrar-se.

**Pode ser testada independentemente:** Sim. A página de registro é acessível sem autenticação.

**Cenários de aceite:**

1. **Dado** que estou na página de registro, **quando** digito um email único, uma senha com mais de 6 caracteres, confirmo a senha corretamente e clico em registrar, **então** vejo uma notificação de sucesso e sou redirecionado para a página de login.

2. **Dado** que estou na página de registro, **quando** digito um email que já existe no sistema, **então** vejo uma mensagem de erro "Email já cadastrado".

3. **Dado** que estou na página de registro, **quando** digito uma senha e sua confirmação não coincide, **então** vejo uma mensagem de erro "Senhas não conferem".

4. **Dado** que estou na página de registro, **quando** clico em "Entrar" ou no link de login, **então** sou redirecionado para a página de login.

---

#### HU-003 — Usuário recebe feedback de validação em tempo real

Enquanto preenche formulários de autenticação, o usuário vê erros desaparecerem conforme corrige os dados.

**Pode ser testada independentemente:** Sim. A validação acontece em todos os formulários.

**Cenários de aceite:**

1. **Dado** que estou em qualquer formulário de autenticação, **quando** digito um valor inválido em um campo (ex: email sem @), **então** um ícone de erro e mensagem aparecem imediatamente.

2. **Dado** que estou em qualquer formulário e vejo um erro, **quando** corrijo o valor para ser válido, **então** o erro desaparece imediatamente.

3. **Dado** que estou em qualquer formulário, **quando** deixo um campo obrigatório vazio e o campo perde o foco, **então** vejo um erro "Campo obrigatório".

---

#### HU-004 — Usuário solicita recuperação de senha

Usuário que esqueceu a senha acessa a página de login, clica em "Esqueceu sua senha?", digita seu email registrado e recebe confirmação de que um link foi enviado.

**Pode ser testada independentemente:** Sim. O modal de recuperação é acessível da página de login.

**Cenários de aceite:**

1. **Dado** que estou na página de login, **quando** clico em "Esqueceu sua senha?", **então** um modal abre com um campo de email.

2. **Dado** que o modal de recuperação está aberto, **quando** digito um email registrado e clico em enviar, **então** vejo uma mensagem "Link de reset enviado para seu email".

3. **Dado** que o modal está aberto, **quando** clico fora dele ou em "Voltar", **então** o modal fecha e retorno à página de login.

4. **Dado** que o modal está aberto, **quando** digito um email que não está registrado, **então** vejo um erro "Email não encontrado".

---

#### HU-005 — Usuário redefine sua senha

Usuário clica no link recebido por email (contendo token válido), acessa a página de reset de senha, digita uma nova senha e confirmação, e consegue fazer login com a nova senha.

**Pode ser testada independentemente:** Sim. A página é acessível via URL com token de reset.

**Cenários de aceite:**

1. **Dado** que acesso a página de reset com um token válido, **quando** digito uma nova senha com mais de 6 caracteres, confirmo a senha corretamente e clico em "Redefinir", **então** vejo uma mensagem de sucesso e sou redirecionado para login.

2. **Dado** que estou na página de reset, **quando** digito uma senha com menos de 7 caracteres, **então** vejo um erro de comprimento antes de conseguir enviar.

3. **Dado** que estou na página de reset, **quando** os campos de senha e confirmação não coincidem, **então** vejo um erro "Senhas não conferem".

4. **Dado** que acesso a página de reset com um token inválido ou expirado, **então** vejo uma mensagem de erro e opção de solicitar novo reset.

---

#### HU-006 — Usuário realiza logout

Usuário autenticado clica em um botão/link de logout, sua sessão é encerrada e é redirecionado para a página de login.

**Pode ser testada independentemente:** Sim. O logout é uma ação isolada.

**Cenários de aceite:**

1. **Dado** que sou um usuário autenticado, **quando** clico em "Sair" ou "Logout", **então** meu token é removido do sessionStorage e sou redirecionado para login.

2. **Dado** que estou na página de login após logout, **quando** tento acessar uma página protegida sem fazer login, **então** permaneço na página de login.

---

### Regras de Negócio

- **RN-001:** Um email é único no sistema; não é permitido registrar duas contas com o mesmo email.
- **RN-002:** Senha deve ter no mínimo 7 caracteres (validação frontend) e no mínimo 8 caracteres no backend.
- **RN-003:** Confirmação de email é realizada automaticamente no backend após o registro; o usuário não precisa validar no frontend.
- **RN-004:** O token JWT é armazenado em sessionStorage (limpo ao fechar o navegador) por questões de segurança.
- **RN-005:** Todos os endpoints de autenticação requerem HTTPS em produção.
- **RN-006:** Erros de validação de formulário são exibidos inline abaixo de cada campo, não em modal.
- **RN-007:** Mensagens de sucesso são exibidas como toasts (notificações passageiras) no topo/canto da página.

---

### Requisitos Funcionais

#### O que o sistema exibe ao ser acessado

**Página de Login:**
- Branding/logo CatDog no topo.
- Dois campos de input: "Email" e "Senha".
- Link "Esqueceu sua senha?" (abre modal).
- Botão púrpura "Entrar".
- Link "Não tem uma conta? Cadastre-se" (leva à página de registro).

**Página de Registro:**
- Branding CatDog.
- Três campos de input: "Email", "Senha", "Confirme sua Senha".
- Botão "Registrar" (púrpuro).
- Link "Já tem uma conta? Entrar" (leva ao login).

**Página de Reset de Senha:**
- Branding CatDog.
- Campo "Nova Senha" e "Confirme sua Senha".
- Botão "Redefinir Senha" (púrpuro).

#### Ações disponíveis

**Ação 1 — Validar Campos em Tempo Real**

Conforme o usuário digita em um campo:
- Email: Verifica se contém "@". Se não, exibe erro "Email inválido".
- Senha: Verifica comprimento >= 7. Se não, exibe erro "Senha deve ter no mínimo 7 caracteres".
- Confirmação de Senha (registro/reset): Verifica igualdade com campo Senha. Se diferente, exibe erro "Senhas não conferem".

---

**Ação 2 — Enviar Formulário de Login**

Quando o usuário clica em "Entrar":
1. Valida campos (email com @, senha >= 7 chars).
2. Se inválido, exibe erros inline.
3. Se válido, desabilita o botão, mostra spinner/estado de carregamento.
4. Faz POST para `/auth/login` com { email, password }.
5. Se sucesso (200): Armazena token em sessionStorage, exibe toast "Bem-vindo!", redireciona conforme lógica de app routing.
6. Se erro 401: Exibe "Email ou senha inválidos".
7. Se erro de servidor: Exibe "Erro ao conectar. Tente novamente".

---

**Ação 3 — Enviar Formulário de Registro**

Quando clica em "Registrar":
1. Valida campos (email, senha >= 7, senhas coincidem).
2. Se inválido, exibe erros.
3. Se válido, desabilita botão, mostra spinner.
4. Faz POST para `/auth/register` com { email, password }.
5. Se sucesso (200/201): Exibe toast "Conta criada! Faça login.", redireciona para login.
6. Se erro 409 (email existe): Exibe "Email já cadastrado".
7. Se outro erro: Exibe mensagem genérica de erro.

---

**Ação 4 — Abrir/Fechar Modal de Recuperação de Senha**

Clique em "Esqueceu sua senha?":
- Abre modal overlay com campo de email.
- Modal tem botão "Enviar" e opção de fechar (X ou clique fora).

Clique em "Enviar" no modal:
1. Valida email (contém @).
2. Se inválido, exibe erro inline.
3. Se válido, desabilita botão, mostra spinner.
4. Faz POST para `/auth/forgot-password` com { email }.
5. Se sucesso: Exibe toast "Link enviado para seu email", fecha modal.
6. Se email não existe: Exibe "Email não encontrado".

---

**Ação 5 — Enviar Reset de Senha**

Na página de reset (acesso via URL com parâmetro token):
1. Extrai token da URL (`?token=xyz`).
2. Valida: senha >= 7, senhas coincidem.
3. Se inválido, exibe erros.
4. Se válido, desabilita botão, mostra spinner.
5. Faz POST para `/auth/reset-password` com { token, newPassword }.
6. Se sucesso: Exibe toast "Senha redefinida!", redireciona para login.
7. Se erro 400/401 (token inválido): Exibe "Link inválido ou expirado. Solicite um novo link".

---

**Ação 6 — Logout**

Clique em botão de logout (fora desta feature, mas integração esperada):
1. Remove token do sessionStorage.
2. Limpa estado da aplicação.
3. Redireciona para página de login.

---

#### Validações e Restrições

- **Email:** Obrigatório, deve conter "@", máximo 255 caracteres.
- **Senha:** Obrigatório, mínimo 7 caracteres, máximo 128 caracteres.
- **Confirmação de Senha (registro/reset):** Deve coincidir exatamente com o campo Senha.
- **Botão de envio:** Desabilitado durante requisição (estado de loading).
- **Modal de recuperação:** Fecha ao clicar fora ou no botão X.
- **Campo de email em modal:** Obrigatório, validação em tempo real (@ requerido).

---

#### Mensagens ao Usuário

| Condição | Mensagem | Tipo |
|---|---|---|
| Email sem @ | "Email inválido" | Erro inline |
| Senha < 7 chars | "Senha deve ter no mínimo 7 caracteres" | Erro inline |
| Senhas não coincidem (registro/reset) | "Senhas não conferem" | Erro inline |
| Campo obrigatório vazio (ao sair do campo) | "Campo obrigatório" | Erro inline |
| Login com sucesso | "Bem-vindo!" | Toast sucesso |
| Registro com sucesso | "Conta criada! Faça login." | Toast sucesso |
| Email/senha inválidos | "Email ou senha inválidos" | Toast erro ou inline |
| Email já registrado | "Email já cadastrado" | Toast/inline erro |
| Forgot password com sucesso | "Link enviado para seu email" | Toast sucesso |
| Email não encontrado (forgot) | "Email não encontrado" | Toast/inline erro |
| Reset de senha com sucesso | "Senha redefinida!" | Toast sucesso |
| Token inválido/expirado | "Link inválido ou expirado. Solicite um novo." | Mensagem em página |
| Erro de conexão genérico | "Erro ao conectar. Tente novamente." | Toast erro |

---

#### Integrações

| Sistema externo | O que é enviado | O que é recebido | Em caso de falha |
|---|---|---|---|
| POST /auth/login | { email, password } | { token, user: { id, email, role } } | Exibe "Email ou senha inválidos" |
| POST /auth/register | { email, password } | { user: { id, email }, message } | Exibe erro (email existe ou outro) |
| POST /auth/forgot-password | { email } | { message: "Email enviado" } | Exibe "Email não encontrado" |
| POST /auth/reset-password | { token, newPassword } | { message: "Senha redefinida" } | Exibe "Link inválido ou expirado" |
| POST /auth/logout | _{vazio}_ | _{vazio}_ | Apenas remove token local |

---

### Requisitos Não Funcionais

| ID | Tipo | Requisito | Critério mensurável |
|---|---|---|---|
| RNF-001 | Desempenho | Formulários respondem com feedback de validação instantaneamente | < 100ms entre digitação e exibição de erro |
| RNF-002 | Desempenho | Submissão de formulário não trava UI | Componentes permanecem responsivos durante loading |
| RNF-003 | Responsividade | Funciona em mobile, tablet e desktop | Testado em viewports: 320px, 768px, 1024px |
| RNF-004 | Acessibilidade | Compatibilidade básica com leitores de tela | Labels associados a campos, ordem de tab lógica |
| RNF-005 | Segurança | Tokens armazenados seguramente | Utiliza sessionStorage (não localStorage), HttpOnly nas cookies |
| RNF-006 | Compatibilidade | Funciona em navegadores modernos | Chrome, Firefox, Safari, Edge últimas 2 versões |
| RNF-007 | Manutenibilidade | Código segue padrões Angular e projeto | Components reutilizáveis, services para lógica, lazy loading de módulos |
| RNF-008 | Qualidade | Cobertura de testes | Mínimo 80% de cobertura em componentes e serviços |

---

### O que Não Deve Ser Feito

- A feature não deve implementar confirmação de email no frontend (é automática no backend).
- A feature não deve implementar route guards nesta versão (será próxima task).
- A feature não deve implementar "Remember Me" (checkbox de manter logado).
- A feature não deve fazer logout no backend via chamada API (token apenas removido localmente).
- A feature não deve armazenar senha em localStorage ou variáveis globais.
- A feature não deve implementar 2FA ou MFA nesta versão.

---

## Grupo 4 — Validação

### Casos de Teste

| ID | Cenário | Entrada | Resultado esperado | Tipo |
|---|---|---|---|---|
| CT-001 | Email válido e senha válida no login | "user@example.com", "password123" | Token armazenado, toast de sucesso | Positivo |
| CT-002 | Email sem @ no login | "user.example.com", "password123" | Erro inline "Email inválido" | Negativo |
| CT-003 | Senha muito curta no login | "user@example.com", "123" | Erro inline "Senha muito curta" | Negativo |
| CT-004 | Email e senha válidos no registro com novo email | "newuser@ex.com", "password123", "password123" | Toast sucesso, redirecionado para login | Positivo |
| CT-005 | Registro com email duplicado | "existing@ex.com", "password123", "password123" | Erro "Email já cadastrado" | Negativo |
| CT-006 | Registro com senhas diferentes | "newuser@ex.com", "password123", "password456" | Erro "Senhas não conferem" | Negativo |
| CT-007 | Validação em tempo real — email sem @ | Digitando "user.example.com" | Erro aparece antes de submeter | Borda |
| CT-008 | Validação em tempo real — corrigi email | "user.example.com" → "user@example.com" | Erro desaparece imediatamente | Borda |
| CT-009 | Modal de forgot password — abrir e fechar | Clique em "Esqueceu senha?" → clique no X | Modal abre e fecha corretamente | Positivo |
| CT-010 | Forgot password com email válido | "user@example.com" no modal | Toast "Link enviado para seu email" | Positivo |
| CT-011 | Forgot password com email não registrado | "nonexistent@ex.com" no modal | Erro "Email não encontrado" | Negativo |
| CT-012 | Reset de senha com token válido | token=xyz, nova_senha="newpass123" | Toast de sucesso, redirecionado para login | Positivo |
| CT-013 | Reset com token inválido/expirado | token=invalid, qualquer senha | Erro "Link inválido ou expirado" | Negativo |
| CT-014 | Responsividade — mobile (320px) | Acessa login em viewport 320px | Layout centralizado, tudo visível, funcional | Positivo |
| CT-015 | Responsividade — tablet (768px) | Acessa login em viewport 768px | Layout mantém proporções, botões acessíveis | Positivo |
| CT-016 | Submissão com loading — formulário não trava | Submete login, durante processamento tenta clicar em campo | Input pode receber foco mas não pode submeter novamente | Borda |

---

### Critérios de Aceite

**Comportamento e entrega:**
- [ ] CA-001: A página de login exibe corretamente em mobile, tablet e desktop.
- [ ] CA-002: Validação de email (requer @) funciona em tempo real em todos os formulários.
- [ ] CA-003: Validação de senha (mín. 7 chars) funciona em tempo real.
- [ ] CA-004: Botão de envio fica desabilitado durante requisição HTTP.
- [ ] CA-005: Toast de sucesso aparece após login/registro/reset bem-sucedidos.
- [ ] CA-006: Modal de "Esqueceu Senha" abre e fecha corretamente.
- [ ] CA-007: Tokens são armazenados em sessionStorage, não em localStorage.
- [ ] CA-008: Erros de validação desaparecem quando dados ficam válidos.
- [ ] CA-009: Campo "Confirme Senha" valida igualdade com "Senha" em tempo real.
- [ ] CA-010: Email duplicado no registro retorna erro específico "Email já cadastrado".

**Regressão:**
- [ ] FEATURE-001 (Backend API) — Endpoints `/auth/login`, `/auth/register`, `/auth/forgot-password`, `/auth/reset-password` continuam funcionando.

**Qualidade de código (SonarQube):**
- [ ] Quality Gate aprovado sem bloqueadores.
- [ ] Cobertura de testes: mínimo de 80% nos componentes de autenticação.
- [ ] Zero issues de segurança (Severity: Blocker ou Critical).
- [ ] Sem hard-coded secrets ou senhas nos arquivos.

---

### Critério de Sucesso da Feature

| Métrica | Baseline atual | Meta após entrega | Como será medida |
|---|---|---|---|
| Usuários completando registro | 0 | >80% conversão | Analytics ou logs de sucesso |
| Tempo para realizar login | N/A | < 2s (incluindo validação) | Timers no frontend |
| Taxa de erro em submissões | N/A | < 5% (excluindo erros de credenciais) | Logs de erro capturados |
| Cobertura de testes | 0% | 80%+ | Relatórios de cobertura do Jest/Karma |

---

## Grupo 5 — Estimativa

> Preencha após o escopo completo estar definido e revisado.

**Use Points gerados:** _A ser definido em reunião de planejamento_
**Estimativa de custo:** _A ser definido_
