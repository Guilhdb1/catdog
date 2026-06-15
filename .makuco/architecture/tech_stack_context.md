# Stack de Tecnologia

## Linguagem e Runtime

| Item | Tecnologia | Versão | Observação |
|---|---|---|---|
| Linguagem principal | C# | Compatível com .NET 10 | Linguagem principal utilizada no backend da aplicação. |
| Runtime / Plataforma | .NET | 10 | Plataforma de execução do backend em ASP.NET Core. |
| Gerenciador de pacotes | npm | Versão estável compatível com Angular 21 | Utilizado para gerenciar dependências do frontend Angular. |

---

## Frameworks e Bibliotecas Principais

| Camada | Framework / Biblioteca | Versão | Finalidade |
|---|---|---|---|
| Backend | ASP.NET Core | Compatível com .NET 10 | Construção da API REST e implementação das regras de negócio do sistema. |
| Frontend | Angular | 21 | Construção da interface web pública e da área administrativa autenticada. |
| ORM / Acesso a dados | Entity Framework Core | Compatível com .NET 10 | Mapeamento objeto-relacional e acesso ao banco de dados MySQL. |
| Testes | xUnit | Versão estável atual | Testes automatizados do backend. |
| Testes | Jasmine + Karma | Versão estável atual | Testes automatizados do frontend Angular. |

---

## Banco de Dados

| Tipo | Tecnologia | Versão | Uso no sistema |
|---|---|---|---|
| Relacional | MySQL | 8.4 | Armazenamento principal de dados do sistema, incluindo espécies, animais, solicitações de adoção e dados administrativos. |
| Cache | Não definido | Não definido | Não há uso de cache definido no momento. |
| Busca | Não definido | Não definido | Não há mecanismo de busca especializado definido no momento. |

---

## Infraestrutura e Cloud

| Item | Tecnologia | Observação |
|---|---|---|
| Cloud provider | Ainda não definido | Provedor de hospedagem ainda não escolhido. |
| Containers | Docker | Será utilizado para empacotamento e execução padronizada da aplicação. |
| Orquestração | Ainda não definido | Não há ferramenta de orquestração definida no momento. |
| CI/CD | Ainda não definido | Pipeline de integração e entrega contínua ainda não definido. |
| Monitoramento | SonarQube | Utilizado para análise de qualidade de código. |

---

## Sistemas e Componentes Externos

No momento, não há integrações externas previstas para o projeto Catdog.

---

## Ferramentas de Desenvolvimento

| Ferramenta | Finalidade |
|---|---|
| Visual Studio | Desenvolvimento principal do backend em .NET. |
| VS Code | Desenvolvimento do frontend, edição geral de código e apoio ao trabalho no monorepo. |
| Docker | Execução local padronizada e empacotamento dos serviços. |
| SonarQube | Análise estática e acompanhamento da qualidade do código. |
| npm | Gerenciamento de dependências e scripts do frontend Angular. |
