# FEATURE-001 — Cadastrar Espécie

---

## Grupo 1 — Identificação

**Feature:** FEATURE-001 — Cadastrar Espécie
**Módulo:** MODULE-001 — Cadastro de Espécies
**Status:** Rascunho
**Criado por:** Guilherme Santos — 2026-06-17
**Aprovado por:** _A preencher_

---

## Objetivo da Feature

Permitir que os Administradores da ONG Catdog registrem novas espécies de animais no sistema. A espécie é o cadastro mais simples e fundamental da plataforma: contém apenas o nome e serve como referência obrigatória para o vínculo com os animais. Sem ao menos uma espécie cadastrada, nenhum animal pode ser registrado. Esta feature garante padronização mínima dos dados, evita inconsistências na classificação dos animais e habilita o restante do fluxo administrativo.

---

## Grupo 2 — Contexto

### Quem Acessa

| Perfil / Permissão | Nível de acesso | Observação |
|---|---|---|
| Administrador (role ADMIN) | Escrita | Único perfil autorizado a cadastrar espécies |
| Adotante (role ADOTANTE) | Sem acesso | Acesso negado; qualquer tentativa deve ser rejeitada |
| Visitante não autenticado | Sem acesso | Acesso negado; qualquer tentativa deve ser rejeitada |

---

### Premissas

- O Administrador já está autenticado na plataforma ao acessar esta funcionalidade.
- O sistema de autenticação com roles ADMIN e ADOTANTE (FEATURE-001 do módulo de Autenticação) já está implementado e funcional.
- Não existe lista inicial de espécies pré-carregadas — o Administrador cadastra todas as espécies manualmente do zero.
- O sistema opera para uma única ONG, portanto a unicidade do nome de espécie é global (não há separação por organização).
- Remoção de espaços nas extremidades (trim) é aplicada ao nome antes de qualquer validação.

---

### Dependências

| Dependência | Tipo | Status | Impacto se não resolvida |
|---|---|---|---|
| Autenticação e controle de acesso por role (ADMIN) | FEATURE — Módulo Authentication | Resolvida | Sem autenticação funcional, o sistema não consegue identificar o Administrador nem bloquear acessos não autorizados |

---

### Referências e Insumos

**Artefatos consultados:**
- `.makuco/overview/glossary_context.md` — definição de Espécie, relacionamentos entre Espécie e Animal
- `.makuco/overview/project_goal_context.md` — perfis de usuário, objetivo do sistema, restrições
- `.makuco/product/scope_features_context.md` — definição desta feature no roadmap do produto

**Tabelas de banco de dados:** `species` (a criar nesta feature)

---

## Grupo 3 — Comportamento

### Histórias de Usuário

---

#### HU-01 — Cadastrar nova espécie

Como Administrador, quero cadastrar uma nova espécie informando apenas o nome, para que ela fique disponível como referência obrigatória ao registrar animais.

**Pode ser testada independentemente:** Sim. Basta acessar a funcionalidade de cadastro de espécie como Administrador autenticado, submeter um nome válido e verificar que a espécie fica disponível para seleção no cadastro de animais.

**Cenários de aceite:**

1. **Dado** que o Administrador está autenticado, **quando** ele submete um nome de espécie válido (não vazio, com 2 a 50 caracteres após trim, ainda não cadastrado), **então** o sistema registra a espécie e retorna confirmação de sucesso.

2. **Dado** que o Administrador submete um nome de espécie já existente (independentemente de maiúsculas ou minúsculas), **quando** tenta realizar o cadastro, **então** o sistema rejeita a operação e informa que a espécie já está cadastrada.

3. **Dado** que o Administrador submete um nome vazio ou composto apenas por espaços em branco, **quando** tenta realizar o cadastro, **então** o sistema rejeita a operação e informa que o nome é obrigatório.

4. **Dado** que o Administrador submete um nome com menos de 2 caracteres (após remoção de espaços nas extremidades), **quando** tenta realizar o cadastro, **então** o sistema rejeita a operação e informa que o nome deve ter no mínimo 2 caracteres.

5. **Dado** que o Administrador submete um nome com mais de 50 caracteres, **quando** tenta realizar o cadastro, **então** o sistema rejeita a operação e informa que o nome deve ter no máximo 50 caracteres.

---

#### HU-02 — Acesso restrito a Administrador

Como responsável pela ONG, quero garantir que somente Administradores consigam cadastrar espécies, para que adotantes ou visitantes não autorizados não interfiram no cadastro interno.

**Pode ser testada independentemente:** Sim. Basta tentar acessar a funcionalidade de cadastro de espécie com um usuário Adotante autenticado ou sem autenticação, e verificar que o acesso é negado.

**Cenários de aceite:**

1. **Dado** que um usuário com perfil Adotante está autenticado, **quando** tenta acessar a funcionalidade de cadastro de espécie, **então** o sistema nega o acesso.

2. **Dado** que um visitante não está autenticado, **quando** tenta acessar a funcionalidade de cadastro de espécie, **então** o sistema nega o acesso.

---

### Regras de Negócio

- **RN-01:** O nome da espécie é o único campo do cadastro. Nenhum outro atributo deve ser solicitado ou aceito nesta feature.
- **RN-02:** O nome da espécie é obrigatório. Nomes vazios ou compostos apenas por espaços em branco não são aceitos.
- **RN-03:** O nome da espécie deve ter no mínimo 2 caracteres, desconsiderando espaços nas extremidades.
- **RN-04:** O nome da espécie deve ter no máximo 50 caracteres.
- **RN-05:** O nome da espécie deve ser único no sistema. A verificação de unicidade ignora diferença entre maiúsculas e minúsculas — "Cachorro", "cachorro" e "CACHORRO" são considerados nomes equivalentes.
- **RN-06:** Somente o Administrador (role ADMIN) pode cadastrar espécies. Adotantes e visitantes não autenticados são rejeitados.
- **RN-07:** A espécie cadastrada com sucesso fica imediatamente disponível como referência para o cadastro de animais, sem necessidade de aprovação ou etapa adicional.

---

### Requisitos Funcionais

#### O que o sistema exibe ao ser acessado

O Administrador acessa a funcionalidade de cadastro de espécie e encontra um formulário com um único campo de entrada: o nome da espécie. Nenhum dado pré-preenchido é exibido. O formulário possui uma ação de submissão para registrar a espécie.

#### Ações disponíveis

**Ação 1 — Submeter nome de espécie para cadastro**

O Administrador preenche o campo de nome e submete o formulário. O sistema aplica trim (remoção de espaços nas extremidades) no nome recebido e executa as validações na seguinte ordem:

Regras condicionais:
- Se o nome, após trim, estiver vazio → o sistema rejeita e retorna mensagem de campo obrigatório.
- Se o nome, após trim, tiver menos de 2 caracteres → o sistema rejeita e retorna mensagem de tamanho mínimo.
- Se o nome tiver mais de 50 caracteres → o sistema rejeita e retorna mensagem de tamanho máximo.
- Se já existir uma espécie com o mesmo nome (sem distinção de caixa) → o sistema rejeita e retorna mensagem de espécie já cadastrada.
- Se todas as validações passarem → o sistema registra a espécie e retorna confirmação de sucesso.

---

#### Validações e Restrições

- O campo **nome** é obrigatório.
- O campo **nome** aceita no mínimo **2** e no máximo **50** caracteres (contados após remoção de espaços nas extremidades).
- O campo **nome** deve ser único no sistema, sem distinção entre maiúsculas e minúsculas.
- Nenhum outro campo além de **nome** deve estar disponível neste formulário.
- A funcionalidade de cadastro não é exibida para perfis Adotante nem para visitantes não autenticados.

---

#### Mensagens ao Usuário

| Condição | Mensagem |
|---|---|
| Cadastro realizado com sucesso | 'Espécie cadastrada com sucesso.' |
| Nome vazio ou apenas espaços | 'O nome da espécie é obrigatório.' |
| Nome com menos de 2 caracteres | 'O nome da espécie deve ter no mínimo 2 caracteres.' |
| Nome com mais de 50 caracteres | 'O nome da espécie deve ter no máximo 50 caracteres.' |
| Nome já cadastrado no sistema | 'Já existe uma espécie com este nome.' |
| Acesso negado (perfil não autorizado) | 'Acesso não autorizado.' |

---

### Requisitos Não Funcionais

| ID | Tipo | Requisito | Critério mensurável |
|---|---|---|---|
| RNF-01 | Segurança | Apenas Administradores autenticados podem cadastrar espécies | 0% de acessos bem-sucedidos por Adotantes ou visitantes não autenticados |
| RNF-02 | Consistência de dados | Nenhuma espécie duplicada deve existir no sistema | 0% de registros com mesmo nome (case-insensitive) após qualquer operação de cadastro |
| RNF-03 | Disponibilidade imediata | A espécie cadastrada deve estar disponível para uso imediato | A espécie aparece como opção de seleção no cadastro de animais imediatamente após o cadastro |

---

### O que Não Deve Ser Feito

- Esta feature não realiza edição de espécies já cadastradas.
- Esta feature não realiza exclusão ou inativação de espécies.
- Esta feature não lista ou exibe espécies existentes (escopo de feature separada).
- Esta feature não aceita campos adicionais além do nome (ex.: descrição, ícone, categoria).
- Esta feature não realiza cadastro em lote nem importação de espécies.
- Esta feature não tem acesso por perfil Adotante nem por visitantes não autenticados.

---

## Grupo 4 — Validação

### Casos de Teste

| ID | Cenário | Entrada | Resultado esperado | Tipo |
|---|---|---|---|---|
| CT-01 | Nome válido | "Cachorro" | Espécie cadastrada com sucesso | Positivo |
| CT-02 | Nome no limite mínimo | "Gá" (2 chars) | Espécie cadastrada com sucesso | Borda |
| CT-03 | Nome no limite máximo | Nome com exatamente 50 caracteres | Espécie cadastrada com sucesso | Borda |
| CT-04 | Nome com espaços nas extremidades | " Coelho " | Espécie cadastrada com nome "Coelho" (trim aplicado) | Positivo |
| CT-05 | Nome vazio | "" | Erro: nome obrigatório | Negativo |
| CT-06 | Nome com apenas espaços em branco | "   " | Erro: nome obrigatório | Negativo |
| CT-07 | Nome com 1 caractere após trim | "G" | Erro: nome abaixo do mínimo | Negativo |
| CT-08 | Nome com 51 caracteres | String com 51 chars | Erro: nome acima do máximo | Negativo |
| CT-09 | Nome duplicado exato | "Cachorro" (já existe "Cachorro") | Erro: espécie já cadastrada | Negativo |
| CT-10 | Nome duplicado em caixa baixa | "cachorro" (já existe "Cachorro") | Erro: espécie já cadastrada | Negativo |
| CT-11 | Nome duplicado em caixa alta | "CACHORRO" (já existe "Cachorro") | Erro: espécie já cadastrada | Negativo |
| CT-12 | Acesso por Adotante autenticado | Perfil Adotante tenta cadastrar | Acesso negado | Negativo |
| CT-13 | Acesso sem autenticação | Sem credencial | Acesso negado | Negativo |
| CT-14 | Nome com 2 chars após trim | " Ab " | Espécie cadastrada com sucesso | Borda |
| CT-15 | Nome com caracteres especiais | "Ave-Exótica" | Espécie cadastrada com sucesso | Positivo |
| CT-16 | Dois cadastros válidos sequenciais | "Cachorro" e depois "Gato" | Ambas as espécies registradas com sucesso | Positivo |

---

### Critérios de Aceite

**Comportamento e entrega:**
- [ ] CA-01: O Administrador autenticado consegue cadastrar uma espécie com nome válido em uma única ação de submissão.
- [ ] CA-02: Após cadastro bem-sucedido, o sistema exibe mensagem de confirmação "Espécie cadastrada com sucesso."
- [ ] CA-03: A espécie cadastrada fica imediatamente disponível como referência para o cadastro de animais.
- [ ] CA-04: O sistema rejeita nomes vazios ou compostos apenas por espaços em branco, exibindo "O nome da espécie é obrigatório."
- [ ] CA-05: O sistema rejeita nomes com menos de 2 caracteres (após trim), exibindo "O nome da espécie deve ter no mínimo 2 caracteres."
- [ ] CA-06: O sistema rejeita nomes com mais de 50 caracteres, exibindo "O nome da espécie deve ter no máximo 50 caracteres."
- [ ] CA-07: O sistema rejeita cadastros de espécies cujo nome (case-insensitive) já existe, exibindo "Já existe uma espécie com este nome."
- [ ] CA-08: Usuários com perfil Adotante recebem acesso negado ao tentar usar a funcionalidade.
- [ ] CA-09: Visitantes não autenticados recebem acesso negado ao tentar usar a funcionalidade.
- [ ] CA-10: O formulário de cadastro possui somente o campo de nome — nenhum outro campo é exibido.

**Regressão:**
- [ ] FEATURE-001 (Authentication) — a restrição de acesso por role depende do sistema de autenticação; mudanças no módulo de autenticação podem impactar o controle de acesso desta feature.

**Qualidade de código (SonarQube):**
- [ ] Quality Gate aprovado sem bloqueadores
- [ ] Cobertura de testes: mínimo de 80% nas classes alteradas
- [ ] Zero issues de segurança (Severity: Blocker ou Critical)

---

### Critério de Sucesso da Feature

| Métrica | Baseline atual | Meta após entrega | Como será medida |
|---|---|---|---|
| Espécies cadastradas com sucesso por Administradores | 0 (nenhum cadastro possível) | 100% dos nomes válidos submetidos resultam em cadastro | Verificação manual de cenários de teste pós-entrega |
| Taxa de rejeição de dados inválidos | N/A | 100% das submissões inválidas rejeitadas com mensagem descritiva | Execução dos casos de teste CT-05 a CT-13 |
| Espécies duplicadas no sistema | N/A | 0 espécies com mesmo nome (case-insensitive) | Consulta ao repositório de espécies após execução dos testes de duplicidade |
| Acessos não autorizados bem-sucedidos | N/A | 0 acessos de Adotante ou visitante não autenticado | Execução dos casos de teste CT-12 e CT-13 |

---

## Grupo 5 — Estimativa

> Preencha após o escopo completo estar definido e revisado.

**Use Points gerados:** _A preencher_
**Estimativa de custo:** _A preencher_
