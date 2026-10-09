# Finance Control

Sistema de gestão financeira pessoal (aplicação desktop), desenvolvido como
projeto de estudo em C# / .NET com foco em boas práticas de arquitetura.

> 🚧 **Status:** Em desenvolvimento — Etapa 1 de 6 (aplicação desktop).

## Sobre

O Finance Control permite cadastrar categorias e transações (entradas e saídas),
consultar o histórico com filtros e acompanhar um resumo financeiro. O objetivo
do projeto é aplicar, na prática, arquitetura em camadas, o padrão MVVM e acesso
a dados com Entity Framework Core.

## Tecnologias

- **Linguagem:** C#
- **Plataforma:** .NET 8
- **Desktop:** WPF (XAML, MVVM, Data Binding)
- **Acesso a dados:** Entity Framework Core, LINQ, Migrations
- **Banco de dados:** SQLite
- **Versionamento:** Git (fluxo Developer → Pull Request → main)

## Arquitetura

O projeto é dividido em três camadas, com dependências em uma única direção:

```
Core  ←  Data  ←  Desktop
```

- **FinanceControlCore** — domínio: entidades, enums, regras de negócio e
  serviços de cálculo. Não depende de nenhuma outra camada.
- **FinanceControlData** — acesso a dados: `DbContext`, migrations e repositórios
  (padrão Repository). Depende do Core.
- **FinanceControlDesktop** — interface em WPF/MVVM. Depende do Core e do Data.

## Funcionalidades

- Cadastro e listagem de **categorias**
- Cadastro de **transações** (valor, data, descrição, tipo e categoria) com validação
- **Histórico** de transações com filtros por período, tipo e categoria
- **Resumo financeiro** com totais de entradas, saídas e saldo, e extrato por
  categoria, com filtros por período (mês atual, mês anterior ou geral) e por tipo

## Como executar

Pré-requisitos: **.NET 8 SDK** (e, opcionalmente, Visual Studio 2022).

```bash
git clone https://github.com/PedroLaia/FinanceControlRepository.git
```

1. Abra a solution `FinanceControl.slnx`.
2. Defina **FinanceControlDesktop** como projeto de inicialização.
3. Execute (F5). O banco SQLite é criado automaticamente na primeira execução
   (via migrations).

## Roadmap

1. **Etapa 1** — Desktop (WPF + MVVM + SQLite) 🚧
2. Etapa 2 — Persistência consolidada + UI amigável
3. Etapa 3 — Relatórios, gráficos e metas
4. Etapa 4 — Backend / API (ASP.NET Core)
5. Etapa 5 — Web (Angular)
6. Etapa 6 — Mobile (.NET MAUI)