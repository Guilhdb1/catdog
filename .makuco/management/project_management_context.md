# Gestão do Projeto e Ciclo de Desenvolvimento

## Plataforma de Gestão

**Plataforma:** Azure DevOps
**URL / Acesso:** Ainda não foi criado.
**Como solicitar acesso:** Solicitar acesso diretamente ao Guilherme.

---

## Modelo de Organização do Trabalho

| Nível | Nome utilizado | O que representa | Exemplo |
|---|---|---|---|
| 1 — mais alto | Task | Unidade principal de trabalho do projeto neste momento | Cadastro de animal |
| 2 | Não utilizado por enquanto | Não há agrupadores formais definidos nesta fase inicial | — |
| 3 | Não utilizado por enquanto | Não há nível intermediário entre demanda e execução | — |
| 4 — mais baixo | Sub-task | Quebra técnica opcional de uma task quando necessário | Validar campos obrigatórios do cadastro de animal |

---

## Tamanho e Critérios de um PBI

**Tamanho máximo:** Uma task deve ser concluível dentro de até 2 dias úteis por um desenvolvedor, para manter fluxo simples, revisão rápida e previsibilidade dentro do ciclo semanal.

**Um bom PBI deve:**
- Ter critério de aceite claro e verificável
- Poder ser desenvolvido e testado de forma independente
- Ser pequeno o suficiente para caber no ciclo semanal
- Ter valor funcional ou técnico identificado

**Um PBI deve ser quebrado quando:**
- A estimativa ultrapassar 2 dias úteis de trabalho
- Tiver mais de uma responsabilidade principal
- Não puder ser validado de forma independente
- Exigir etapas técnicas distintas que façam sentido como sub-tasks

---

## Modelo de Desenvolvimento

**Metodologia:** Scrumban

**Duração do ciclo:** Ciclos semanais de 1 semana

**Início do ciclo:** No primeiro dia útil de cada semana

---

## Cerimônias e Rituais

| Cerimônia | Frequência | Duração | Objetivo |
|---|---|---|---|
| Planning | Semanal | Aproximadamente 30 a 60 minutos | Planejar o trabalho da semana |
| Review | Semanal | Aproximadamente 30 minutos | Revisar o que foi concluído no ciclo |
| Retrospectiva | Semanal | Aproximadamente 30 minutos | Identificar melhorias no processo |

---

## Fluxo de Status

| Status | Descrição | Quem move para cá |
|---|---|---|
| Backlog | Item criado mas ainda não priorizado | Guilherme |
| Ready | Item priorizado e com detalhamento suficiente para execução | Guilherme |
| In Progress | Item em desenvolvimento | Desenvolvedor responsável |
| In Review | Item finalizado e aguardando revisão | Desenvolvedor responsável |
| Done | Item concluído, testável e aceito | Guilherme |

---

## Definição de Pronto (Definition of Done)

- Item implementado e testável
- Código revisado quando aplicável
- Testes executados com resultado satisfatório
- Deploy em homologação quando aplicável
- Critérios de aceite atendidos

---

## Acompanhamento e Monitoramento

**Responsável pelo acompanhamento:** Guilherme

**Métricas acompanhadas:**

| Métrica | O que mede | Onde é acompanhada | Frequência |
|---|---|---|---|
| Lead Time | Tempo médio entre criação e conclusão dos itens | Azure DevOps, quando estiver disponível | Semanal |

**Reporte para stakeholders:** Acompanhamento simples em cadência semanal, com revisão do andamento durante o ciclo e visibilidade direta com o responsável pelo projeto.