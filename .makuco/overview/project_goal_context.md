# Objetivo do Projeto

## Identificação do Sistema

**Nome do sistema:** Catdog

**Status:** Em desenvolvimento

**Repositório de código:** Ainda não definido

**Última atualização:** 2026-05-25 — Guilherme Santos

### Ambientes

| Ambiente | URL |
|---|---|
| Desenvolvimento | Ainda não definido |
| Homologação | Ainda não definido |
| Produção | Ainda não definido |

---

## Problema a Ser Resolvido

**Situação atual:** Atualmente a ONG Catdog divulga animais para adoção em plataformas externas, como grupos de Facebook e conversas por WhatsApp. As solicitações de adoção chegam de forma descentralizada e são controladas manualmente pela equipe. Isso dificulta tanto a visualização dos animais disponíveis pelos interessados quanto a organização interna das solicitações entre os administradores.

**Causa raiz:** O problema existe porque a ONG não possui um sistema próprio para concentrar o cadastro dos animais, sua divulgação e o controle interno das solicitações de adoção. O processo depende de canais genéricos de comunicação, sem estrutura adequada para registro, acompanhamento e consulta.

**Impacto:** Os 3 administradores da ONG perdem tempo organizando informações manualmente, correm risco de perder solicitações ou duplicar atendimentos e têm menor visibilidade sobre o andamento interno de cada caso. Para os adotantes, a experiência também é prejudicada, pois a visualização dos animais disponíveis depende de canais dispersos e pouco estruturados.

---

## Objetivo do Projeto

**Onde devemos chegar com o projeto entregue:**

- Centralizar o cadastro e o anúncio dos animais disponíveis para adoção em um único sistema da ONG.
- Reduzir a perda de solicitações de adoção causada pelo controle manual em canais externos.
- Facilitar o controle interno das solicitações pelos administradores da Catdog.
- Melhorar a visualização dos animais disponíveis para os adotantes.

---

## Visão Geral do Sistema

### Propósito

Catdog é uma plataforma de adoção de animais de uma única organização, criada para concentrar em um só lugar o cadastro dos animais disponíveis e o registro das solicitações de adoção. O sistema busca substituir a dispersão atual em redes sociais e aplicativos de mensagem por uma operação mais organizada e rastreável. Ele atende tanto a necessidade interna de controle da ONG quanto a necessidade externa de consulta dos animais por possíveis adotantes.

### Público-Alvo e Usuários

**Perfil 1 — Administrador da ONG**  
_Descrição: membro interno da equipe Catdog responsável por cadastrar animais, manter informações atualizadas e controlar internamente as solicitações de adoção._  
_O que faz e quando faz: usa o sistema no dia a dia da operação para cadastrar animais, gerenciar espécies, acompanhar solicitações e manter o controle interno do processo de adoção._

**Perfil 2 — Adotante**  
_Descrição: pessoa interessada em adotar um animal da ONG, que acessa a plataforma para consultar os animais disponíveis e registrar uma solicitação de adoção._  
_O que faz e quando faz: utiliza o sistema quando busca um animal para adoção, navega pela lista de animais disponíveis e envia sua solicitação para análise da ONG._

**Perfil 3 — Responsável pela ONG**  
_Descrição: pessoa com papel de decisão no projeto e na operação da organização, interessada em garantir que o processo de adoção seja mais organizado, controlável e visível._  
_O que faz e quando faz: acompanha o projeto em nível de negócio, valida o funcionamento geral da plataforma e avalia se o sistema atende às necessidades operacionais da ONG._

### Contexto de Mercado e Posicionamento

**Contexto de mercado:** O sistema atua no contexto de adoção de animais por organizações de proteção animal e ONGs. Nesse cenário, é comum que a divulgação dos animais e o contato inicial com interessados aconteçam por redes sociais e aplicativos de mensagem, o que gera processos informais e pouco estruturados.

**Posicionamento:** O Catdog se posiciona como uma plataforma própria de gestão e divulgação de adoções para uma única ONG, focada em simplicidade operacional e centralização das informações. Em vez de ser um marketplace multi-organização ou uma rede social de adoção, o sistema atende a operação específica da Catdog e organiza o processo interno sem depender de múltiplas ferramentas dispersas.

**Público-alvo de mercado:** O produto se destina à própria operação da ONG Catdog e aos interessados em adotar animais cadastrados por ela.

### Contexto de Uso pelo Cliente

A ONG utilizará o Catdog como sistema principal para centralizar o cadastro e a divulgação dos animais, bem como o controle interno das solicitações de adoção. O contato com os interessados continuará ocorrendo fora do sistema, por canais externos como mensagens ou outros meios definidos pela equipe. Neste momento, não há integração com outros sistemas e a operação será concentrada em uma única organização.

---

## Contexto de Negócio

**Sobre o negócio:** A Catdog é uma ONG que atua com adoção de animais e precisa organizar melhor sua operação de divulgação e controle das solicitações recebidas. O projeto nasce de uma necessidade operacional real da organização, que hoje depende de processos manuais e canais dispersos.

**Domínio e segmento:** O sistema está inserido no domínio de adoção de animais e gestão operacional de ONG de proteção animal.

**Processo atual (como as pessoas fazem hoje):** Hoje os animais são anunciados em canais externos, como grupos de Facebook e conversas por WhatsApp. Os interessados entram em contato por fora, e os administradores fazem o acompanhamento das etapas internamente de forma manual. Não existe um sistema único para registrar os animais disponíveis e consolidar as solicitações.

**Restrições e regras de negócio relevantes:** O sistema atenderá somente uma única organização. Não haverá pagamentos. Não haverá comunicação com os adotantes dentro da plataforma por mensagem, notificação ou e-mail; esse contato continuará sendo feito externamente. A plataforma terá gestão interna de solicitações, cadastro de animais e cadastro de espécies com apenas o nome da espécie associado ao animal.

---

## Escopo Macro do Projeto

| # | Módulo / Epic | Prioridade |
|---|---|---|
| 1 | Catálogo de animais disponíveis | Alta |
| 2 | Solicitações de adoção | Alta |
| 3 | Painel administrativo | Alta |
| 4 | Cadastro de espécies | Média |

---

## Escopo Negativo do Projeto

| O que não será feito | Motivo |
|---|---|
| Pagamentos na plataforma | Não faz parte do processo de adoção definido pela ONG |
| Comunicação com adotantes por mensagem, notificação ou e-mail dentro do sistema | O contato continuará sendo feito por canais externos |
| Suporte a múltiplas ONGs ou organizações | O sistema será exclusivo para a operação de uma única organização |

---

## Pessoas e Interesses (Stakeholders)

| Nome | Empresa / Área | Papel no Projeto |
|---|---|---|
| Maria | Catdog / Direção da ONG | Patrocinadora e responsável pelo negócio |
| Guilherme | Desenvolvimento | Desenvolvedor do projeto |
| Administradores da ONG | Catdog / Operação | Usuários internos principais |
| Adotantes | Público externo | Usuários finais da consulta e solicitação de adoção |
