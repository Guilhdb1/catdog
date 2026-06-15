# Definição de Arquitetura

## Padrão Arquitetural Adotado

**Padrão:** Monólito modular

**Justificativa:** O Catdog será desenvolvido por um time pequeno, com escopo funcional relativamente enxuto e foco em entregar valor rapidamente para uma única ONG. Nesse contexto, um monólito modular oferece uma boa relação entre simplicidade de desenvolvimento, baixo custo operacional e organização interna do código. Esse padrão permite manter a aplicação em uma base única, com menos complexidade de infraestrutura do que microserviços, ao mesmo tempo em que preserva separação lógica entre módulos de negócio como espécies, animais, solicitações de adoção e autenticação administrativa. A escolha também favorece evolução incremental, manutenção mais simples e menor esforço de coordenação técnica em um projeto ainda em fase inicial.

---

## Como o Sistema está Organizado

O sistema será mantido em um monorepo com dois serviços principais: um frontend web em Angular e um backend em .NET. O frontend consumirá uma API REST exposta pelo backend, que centralizará as regras de negócio e o acesso ao banco de dados MySQL. Internamente, o backend será organizado por módulos de negócio, refletindo os principais domínios do produto, como espécies, animais, solicitações de adoção e autenticação administrativa. A aplicação terá uma área pública para vitrine dos animais disponíveis e uma área autenticada para uso interno dos administradores da ONG.

---

## Decisões Arquiteturais Importantes

| Decisão | O que foi decidido | Justificativa |
|---|---|---|
| Estrutura da solução | O projeto será mantido em um monorepo contendo frontend e backend separados. | Facilita a gestão centralizada do projeto, simplifica versionamento inicial e reduz dispersão para um time pequeno. |
| Padrão arquitetural do backend | O backend seguirá um modelo de monólito modular organizado por módulos de negócio. | Mantém simplicidade operacional sem abrir mão de separação interna por responsabilidades e domínios. |
| Comunicação entre camadas | O frontend se comunicará com o backend por API REST síncrona via HTTP. | É uma abordagem simples, amplamente suportada e adequada ao porte e ao contexto do sistema web proposto. |
| Persistência | A aplicação utilizará um único banco de dados relacional MySQL. | O domínio atual é simples, a operação é de uma única ONG e não há necessidade identificada de particionamento ou múltiplas bases. |
| Separação de áreas do sistema | O sistema terá área pública para vitrine dos animais e área autenticada para administração interna. | Essa separação reflete diretamente os dois perfis centrais do produto e protege operações internas da ONG. |
| Organização do backend | O backend será dividido em módulos como espécies, animais, solicitações de adoção e autenticação. | A estrutura modular aproxima o código do domínio do negócio e facilita manutenção e evolução futura. |

---

## Diagramas

**C1 — Contexto:** Ainda não gerado.
**C2 — Containers:** Ainda não gerado.
**C3 — Componentes:** Ainda não gerado.

---

Este documento registra a intenção arquitetural inicial do Catdog. Como o projeto ainda está em fase inicial, novas decisões podem surgir ao longo da implementação, mas mudanças relevantes no padrão arquitetural ou nas decisões estruturais devem ser registradas formalmente para manter alinhamento entre produto e implementação.
