# Glossário do Projeto

## Termos do Domínio

| Termo | Tradução EN | Definição | Evitar (sinônimos incorretos) |
|---|---|---|---|
| Administrador | Administrator | Usuário interno da ONG Catdog com acesso à área autenticada da plataforma. É responsável por cadastrar espécies, cadastrar e manter animais, além de registrar e acompanhar internamente as solicitações de adoção. Atua como operador do sistema no dia a dia. | Não há termo proibido definido no momento. |
| Adotante | Adopter | Pessoa interessada em adotar um animal da ONG. Usa a vitrine pública para visualizar os animais disponíveis e pode registrar uma solicitação de adoção para um animal específico. Não realiza gestão interna do sistema. | Não há termo proibido definido no momento. |
| Animal | Animal | Animal registrado pela ONG no sistema para acompanhamento e possível adoção. Possui dados como nome, espécie, idade, descrição, fotos e status de adoção. Cada animal deve estar vinculado a uma espécie cadastrada e pode ou não estar visível na vitrine pública, conforme seu status. | Não há termo proibido definido no momento. |
| Animal adotado | Adopted animal | Estado do animal que indica que ele já foi adotado e, por isso, não deve mais aparecer na vitrine pública de animais disponíveis. Continua existindo no sistema para controle e histórico interno da ONG. | Não há termo proibido definido no momento. |
| Animal disponível | Available animal | Estado do animal que indica que ele está apto para ser exibido na vitrine pública e pode receber novas solicitações de adoção. É o status utilizado quando o animal ainda está disponível para adoção pela ONG. | Não há termo proibido definido no momento. |
| Espécie | Species | Classificação do animal cadastrada no sistema, como cachorro ou gato. É um cadastro simples, composto pelo nome da espécie, e serve como referência obrigatória para o cadastro dos animais. | Não há termo proibido definido no momento. |
| Solicitação de adoção | Adoption request | Registro do interesse de um adotante em um animal específico da ONG. Contém pelo menos nome do interessado, contato, animal relacionado e observações. É utilizada para controle interno da equipe, enquanto o contato com o adotante ocorre fora da plataforma. | Não há termo proibido definido no momento. |
| Status da solicitação | Adoption request status | Situação interna de uma solicitação de adoção dentro do sistema. É usada pelos administradores para acompanhar em que ponto do controle interno a solicitação se encontra. Os status atualmente definidos são aberta, em análise e finalizada. | Não há termo proibido definido no momento. |
| Status do animal | Animal status | Situação do animal no processo de adoção dentro do sistema. Define se ele pode ou não aparecer na vitrine pública. Os status atualmente definidos são disponível e adotado. | Não há termo proibido definido no momento. |

---

## Status e Ciclos de Vida

### Animal

O ciclo de vida do animal no sistema é simples e serve para controlar sua disponibilidade na vitrine pública. Um animal começa como disponível e, quando a adoção é concluída pela ONG, passa para adotado. Animais adotados não devem mais ser exibidos publicamente.

| Status | Descrição | Transições permitidas |
|---|---|---|
| Disponível | Animal apto a aparecer na vitrine pública e receber solicitações de adoção. | Pode avançar para Adotado. |
| Adotado | Animal que já foi adotado e não deve mais aparecer na vitrine pública. | Não há transição definida no momento. |

### Solicitação de adoção

O ciclo de vida da solicitação representa o acompanhamento interno feito pelos administradores. A solicitação é registrada no sistema, passa por análise interna da equipe e depois é encerrada. O contato com o adotante ocorre fora da plataforma, mas o controle do andamento fica registrado internamente.

| Status | Descrição | Transições permitidas |
|---|---|---|
| Aberta | Solicitação registrada no sistema e ainda não iniciada no processo interno de avaliação. | Pode avançar para Em análise ou Finalizada. |
| Em análise | Solicitação que está sendo avaliada internamente pelos administradores da ONG. | Pode avançar para Finalizada. |
| Finalizada | Solicitação com processo interno encerrado pela ONG. | Não há transição definida no momento. |

---

## Relações Entre Termos

- Um animal pertence a exatamente uma espécie cadastrada no sistema.
- Uma espécie pode estar associada a vários animais.
- Um adotante pode registrar uma solicitação de adoção para um animal específico.
- Uma solicitação de adoção sempre se refere a um único animal.
- Um administrador gerencia o cadastro de espécies, animais e solicitações de adoção.
- Apenas animais com status disponível aparecem na vitrine pública.
- Animais com status adotado permanecem no sistema para controle interno, mas deixam de ser exibidos publicamente.

---

## Siglas e Abreviações

No momento, não há siglas ou abreviações de negócio formalmente definidas para o projeto Catdog.

---

## Histórico de Alterações

| Data | Termo | Alteração | Motivo |
|---|---|---|---|
| 2026-05-25 | Administrador, Adotante, Animal, Espécie, Solicitação de adoção, Status do animal, Status da solicitação | Adicionado | Criação inicial do glossário do projeto Catdog. |
| 2026-05-25 | Animal disponível, Animal adotado | Adicionado | Registro explícito dos estados de negócio usados na vitrine pública e no controle interno. |
