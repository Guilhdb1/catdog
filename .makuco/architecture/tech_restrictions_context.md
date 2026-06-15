# Restrições e Decisões Técnicas

## Tecnologias Proibidas

| O que não usar | Motivo | Alternativa recomendada |
|---|---|---|
| Microserviços | O projeto é pequeno neste momento, será desenvolvido por um time enxuto e possui orçamento limitado. A adoção de microserviços aumentaria a complexidade de desenvolvimento, infraestrutura, observabilidade e manutenção sem benefício proporcional ao contexto atual. | Monólito modular |

---

## Restrições de Ambiente

| Restrição | Descrição | Impacto no projeto |
|---|---|---|
| Time pequeno | O desenvolvimento será conduzido por uma equipe enxuta. | As decisões técnicas precisam priorizar simplicidade, produtividade e baixo custo de manutenção. |
| Orçamento limitado | O projeto possui restrição de investimento inicial. | A solução deve evitar complexidade operacional desnecessária, múltiplos serviços e infraestrutura sofisticada. |
| Operação de uma única ONG | O sistema atenderá somente uma organização. | A arquitetura e o modelo de dados não precisam suportar multi-tenant ou múltiplas organizações neste momento. |
| Ambientes ainda não definidos | Infraestrutura, ambientes e provedor cloud ainda não foram formalmente definidos. | As decisões atuais devem manter flexibilidade de implantação e evitar dependência prematura de um fornecedor específico. |

---

## Restrições de Segurança e Compliance

| Requisito | Descrição | Como é atendido |
|---|---|---|
| Controle de acesso administrativo | Apenas administradores da ONG devem acessar a área interna e os dados operacionais da plataforma. | O sistema terá autenticação para administradores e separação entre área pública e área autenticada. |
| Proteção de dados pessoais dos adotantes | O sistema armazenará dados pessoais básicos dos interessados para controle interno da ONG. Esses dados não devem ficar expostos publicamente. | O acesso aos dados dos adotantes será restrito aos administradores autenticados. |
| Criptografia em trânsito | A aplicação deve proteger a comunicação entre cliente e servidor. | O sistema deve operar com HTTPS. |

---

## Decisões Tomadas e Não Reverter

| Decisão | Contexto | Por que não reverter |
|---|---|---|
| Monorepo com frontend e backend | Estrutura definida para manter o projeto centralizado em uma única base de código. | Reverter essa decisão aumentaria a dispersão do projeto, o custo de organização e a complexidade de manutenção para um time pequeno. |
| Frontend em Angular | Tecnologia escolhida para a interface web pública e administrativa do Catdog. | Reverter exigiria recomeço relevante da implementação da interface, treinamento e redefinição das práticas do frontend. |
| Backend em .NET com ASP.NET Core | Stack escolhida para a API REST e para a implementação da lógica de negócio. | Reverter implicaria reescrita significativa da aplicação e perda de alinhamento técnico já estabelecido. |
| Banco de dados MySQL | Banco relacional escolhido para persistência principal do sistema. | Reverter exigiria revisão do modelo de dados, da camada de persistência e do ambiente de execução. |
| Arquitetura em monólito modular | Padrão arquitetural adotado para equilibrar simplicidade operacional e organização por domínio. | Reverter essa decisão sem necessidade forte aumentaria complexidade arquitetural e custo operacional incompatíveis com o porte atual do projeto. |
