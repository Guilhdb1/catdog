# Detalhamento do Escopo Macro do Projeto

## Visão Geral do Produto

Catdog é uma plataforma de adoção de animais de uma única ONG, criada para centralizar o cadastro e a divulgação dos animais disponíveis, além do controle interno das solicitações de adoção. O produto resolve a dispersão atual em canais como Facebook e WhatsApp, onde o processo acontece de forma manual e descentralizada. Quando o projeto estiver completo, a ONG terá um sistema próprio para organizar sua operação de adoção, enquanto os adotantes poderão visualizar os animais disponíveis em uma vitrine pública e registrar solicitações de interesse de forma estruturada.

---

## Roadmap

| Ordem | Módulo | O que entrega ao negócio |
|---|---|---|
| 1 | Cadastro de espécies | Garante a base de classificação dos animais, padronizando o vínculo obrigatório entre cada animal e sua espécie. |
| 2 | Cadastro de animais | Permite que a ONG mantenha um catálogo estruturado dos animais, com controle de disponibilidade para adoção e informações atualizadas para uso interno e público. |
| 3 | Vitrine pública de animais disponíveis | Dá visibilidade aos animais aptos para adoção sem exigir login, melhorando o acesso dos interessados às informações dos animais. |
| 4 | Controle interno de solicitações de adoção | Organiza o registro e acompanhamento interno das solicitações recebidas, reduzindo perda de informação e melhorando a gestão entre os administradores. |

---

## Módulos e Features

---

### Módulo: Cadastro de espécies

Esse módulo resolve a necessidade de manter um cadastro simples e padronizado das espécies dos animais atendidos pela ONG. Ele é utilizado pelos administradores e serve como base obrigatória para o cadastro correto dos animais, evitando inconsistências na classificação.

#### Feature: Cadastrar espécie

Permite que os administradores criem novos registros de espécies de animais no sistema. O cadastro é intencionalmente simples e contém apenas o nome da espécie, pois o objetivo é dar suporte ao vínculo com os animais sem complexidade desnecessária. Essa feature entrega valor ao garantir padronização mínima dos dados e apoiar o restante do fluxo administrativo.

#### Feature: Listar espécies cadastradas

Permite que os administradores visualizem as espécies já registradas para consulta e uso no cadastro de animais. Essa visualização ajuda a evitar duplicidade de cadastros e garante consistência na seleção da espécie relacionada a cada animal. O valor principal é manter uma base simples, reutilizável e controlada para os cadastros internos.

#### Feature: Vincular espécie ao animal

Garante que cada animal cadastrado no sistema esteja associado a uma espécie previamente registrada. Essa regra de negócio diferencia o sistema de um cadastro genérico, pois impede que animais sejam criados sem classificação mínima. O valor entregue é assegurar integridade básica dos dados e melhor organização da vitrine e da gestão interna.

---

### Módulo: Cadastro de animais

Esse módulo permite que a ONG mantenha o registro completo dos animais atendidos e controle quais deles estão disponíveis para adoção. É usado pelos administradores e representa o núcleo operacional da plataforma, pois alimenta tanto a vitrine pública quanto o controle interno.

#### Feature: Cadastrar animal

Permite que os administradores criem o registro de um novo animal no sistema com os campos mínimos definidos: nome, espécie, idade, descrição, status de adoção e fotos. Essa feature organiza a entrada de dados essenciais para a divulgação e o controle da adoção. O valor entregue está em transformar informações hoje dispersas em um cadastro estruturado e reutilizável.

#### Feature: Editar e atualizar dados do animal

Permite que os administradores mantenham os dados do animal atualizados ao longo do tempo, corrigindo informações ou ajustando seu status conforme a situação da adoção evolui. Isso é importante porque as informações exibidas ao público precisam refletir a situação real do animal. O valor entregue é manter a confiabilidade do catálogo e evitar exposição de dados desatualizados.

#### Feature: Remover animal do cadastro

Permite que os administradores excluam registros de animais quando isso fizer sentido para a operação da ONG. Essa feature ajuda a manter o cadastro limpo e aderente à realidade operacional, evitando acúmulo de registros irrelevantes ou incorretos. O valor está na manutenção da qualidade da base de dados e na simplificação da gestão interna.

#### Feature: Controlar status de adoção do animal

Permite que o animal seja marcado como disponível ou adotado, conforme a regra de negócio já definida para o sistema. Apenas animais com status disponível devem aparecer na vitrine pública, o que torna essa feature essencial para conectar o uso interno ao uso externo da plataforma. O valor entregue é garantir coerência entre operação administrativa e divulgação pública.

---

### Módulo: Vitrine pública de animais disponíveis

Esse módulo atende os visitantes e adotantes que desejam conhecer os animais disponíveis para adoção. Ele resolve o problema de visibilidade atual ao concentrar em um ambiente público e organizado as informações principais de cada animal, sem exigir autenticação para consulta.

#### Feature: Listar animais disponíveis publicamente

Permite que qualquer visitante visualize os animais que estão com status disponível no sistema. A listagem pública substitui a dependência de canais dispersos e oferece uma forma centralizada de consulta aos interessados. O valor entregue é ampliar a visibilidade dos animais e tornar o processo inicial de busca mais claro e acessível.

#### Feature: Exibir detalhes do animal na vitrine

Permite que a vitrine pública apresente, para cada animal, pelo menos foto, nome, espécie, idade e descrição. Essas informações ajudam o adotante a entender melhor o perfil do animal antes de demonstrar interesse. O valor para o negócio é melhorar a qualidade da divulgação e reduzir dúvidas básicas no primeiro contato.

#### Feature: Iniciar solicitação de adoção a partir da vitrine

Permite que o visitante avance do interesse no animal para o registro de uma solicitação de adoção diretamente pela plataforma. Essa feature conecta a visualização pública ao processo interno da ONG sem depender exclusivamente de abordagens manuais desde o primeiro momento. O valor está em estruturar a entrada das solicitações e reduzir perda de informações.

---

### Módulo: Controle interno de solicitações de adoção

Esse módulo apoia os administradores no registro e acompanhamento das solicitações recebidas para cada animal. Ele resolve a principal dor operacional relatada pela ONG, que hoje controla esse processo manualmente e de forma descentralizada entre os membros da equipe.

#### Feature: Registrar solicitação de adoção

Permite criar uma solicitação contendo, no mínimo, nome do interessado, contato, animal relacionado e observações. Essa estrutura garante que as informações essenciais de cada interesse fiquem registradas em um único lugar, sem depender de conversas soltas em aplicativos externos. O valor entregue é centralizar o ponto de entrada das solicitações e reduzir perdas operacionais.

#### Feature: Acompanhar solicitações internamente

Permite que os administradores visualizem e acompanhem as solicitações registradas no sistema como parte do controle interno da ONG. Mesmo que o contato com o adotante continue fora da plataforma, essa feature organiza o trabalho da equipe e dá visibilidade compartilhada sobre cada solicitação. O valor entregue é melhorar a coordenação entre os três administradores.

#### Feature: Relacionar solicitação ao animal de interesse

Permite vincular cada solicitação ao animal específico para o qual o adotante demonstrou interesse. Essa relação é essencial para que a ONG entenda a demanda por animal, mantenha rastreabilidade e evite confusão entre atendimentos paralelos. O valor está em dar contexto ao processo interno e tornar a gestão mais confiável.

#### Feature: Registrar observações internas da solicitação

Permite que os administradores adicionem observações relevantes ao acompanhamento interno da solicitação. Como a comunicação com o adotante acontece fora do sistema, esse campo funciona como memória operacional da equipe sobre cada caso. O valor entregue é preservar contexto e apoiar a continuidade do atendimento entre diferentes administradores.

---

### Módulo: Painel administrativo

O painel administrativo é a área autenticada da plataforma utilizada pelos administradores da ONG. Ele não representa um fluxo de negócio separado, mas sim o ambiente onde ficam concentrados os módulos internos de cadastro de espécies, cadastro de animais e controle das solicitações de adoção.

#### Feature: Acessar área autenticada de administração

Permite que apenas administradores tenham acesso às funcionalidades internas de gestão da plataforma. Essa separação entre área pública e área administrativa protege as informações operacionais da ONG e garante que apenas usuários autorizados façam manutenção dos dados. O valor entregue é segurança básica de acesso e organização da operação interna.

#### Feature: Centralizar operações internas em um único ambiente

Permite reunir em uma mesma área autenticada os principais fluxos administrativos do sistema, evitando dispersão entre telas e processos sem relação. Essa centralização facilita o uso diário pelos três administradores da ONG e reforça o papel do sistema como ferramenta principal de controle operacional. O valor entregue é produtividade e clareza de uso.

---

## Fora do Escopo

| Item excluído | Motivo |
|---|---|
| Pagamentos na plataforma | O processo de adoção da ONG não depende de transações financeiras dentro do sistema. |
| Mensagens, notificações e envio de e-mails pelo sistema | O contato com os adotantes continuará sendo feito por canais externos. |
| Suporte a múltiplas ONGs ou organizações | O produto será usado por uma única organização. |
| Painel administrativo como módulo analítico avançado | Neste momento, o painel administrativo será apenas a área autenticada que concentra os cadastros e o controle interno. |
