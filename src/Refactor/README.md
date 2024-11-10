# Projeto de Refatoração

## Visão Geral

Este projeto analisa código C# para identificar oportunidades de refatoração, detectando métodos semelhantes em diferentes classes. Ele gera um relatório sugerindo como refatorar o código para melhorar a manutenibilidade e reduzir a redundância.

## Funcionalidades

- Analisa arquivos C# para encontrar métodos semelhantes.
- Gera um relatório com sugestões de refatoração.
- Utiliza Roslyn para análise de código C#.

## Estrutura do Projeto

- `Refactor/Program.cs`: Ponto de entrada da aplicação.
- `Refactor/Analyzer.cs`: Contém a lógica para analisar o código e encontrar oportunidades de refatoração.
- `Refactor/Report.cs`: Gera um relatório com base na análise.
- `Refactor/dto/Relationship.cs`: Objeto de transferência de dados representando relações entre métodos.
- `Refactor/utils`: Classes e métodos utilitários.

## Começando

### Pré-requisitos

- .NET SDK
- Visual Studio ou JetBrains Rider

### Uso

1. Abra o projeto no seu IDE preferido (por exemplo, JetBrains Rider).
2. Compile o projeto para restaurar as dependências.
3. Execute a aplicação:
    ```sh
    dotnet run --project Refactor
    ```

### Exemplo

Coloque seus arquivos C# no diretório `testes`. A aplicação irá analisar esses arquivos e gerar um relatório com sugestões de refatoração.