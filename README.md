# DIO - Trilha .NET - API e Entity Framework
www.dio.me

## Desafio de projeto
Para este desafio, você precisará usar seus conhecimentos adquiridos no módulo de API e Entity Framework, da trilha .NET da DIO.

## Contexto
Você precisa construir um sistema gerenciador de tarefas, onde você poderá cadastrar uma lista de tarefas que permitirá organizar melhor a sua rotina.

Essa lista de tarefas precisa ter um CRUD, ou seja, deverá permitir a você obter os registros, criar, salvar e deletar esses registros.

A sua aplicação deverá ser do tipo Web API ou MVC, fique a vontade para implementar a solução que achar mais adequado.

A sua classe principal, a classe de tarefa, deve ser a seguinte:

![Diagrama da classe Tarefa](diagrama.png)

Não se esqueça de gerar a sua migration para atualização no banco de dados.

## Métodos esperados
É esperado que você crie o seus métodos conforme a seguir:


**Swagger**


![Métodos Swagger](swagger.png)


**Endpoints**


| Verbo  | Endpoint                | Parâmetro | Body          |
|--------|-------------------------|-----------|---------------|
| GET    | /Tarefa/{id}            | id        | N/A           |
| PUT    | /Tarefa/{id}            | id        | Schema Tarefa |
| DELETE | /Tarefa/{id}            | id        | N/A           |
| GET    | /Tarefa/ObterTodos      | N/A       | N/A           |
| GET    | /Tarefa/ObterPorTitulo  | titulo    | N/A           |
| GET    | /Tarefa/ObterPorData    | data      | N/A           |
| GET    | /Tarefa/ObterPorStatus  | status    | N/A           |
| POST   | /Tarefa                 | N/A       | Schema Tarefa |

Esse é o schema (model) de Tarefa, utilizado para passar para os métodos que exigirem

```json
{
  "id": 0,
  "titulo": "string",
  "descricao": "string",
  "data": "2022-06-08T01:31:07.056Z",
  "status": "Pendente"
}
```


## Solução
O código está pela metade, e você deverá dar continuidade obedecendo as regras descritas acima, para que no final, tenhamos um programa funcional. Procure pela palavra comentada "TODO" no código, em seguida, implemente conforme as regras acima.

---

## Data Analysis API — Integração com ferramentas de BI ✅

- Configure sua connection string em `appsettings.Development.json` usando a chave `ConnectionStrings:ConexaoPadrao`.
- Rode as migrations e atualize o banco:
  - `dotnet ef migrations add InitialCreate` (se necessário)
  - `dotnet ef database update`
- Execute a API:
  - `dotnet run`

**Comandos úteis**

- Instalar EF CLI (se necessário): `dotnet tool install --global dotnet-ef --version 6.0.21`
- Criar migration: `dotnet ef migrations add InitialCreate -o Migrations`
- Aplicar migrations: `dotnet ef database update`
- Definir variável de ambiente temporária (PowerShell): `$env:TRILHA_CONNECTION = "Server=...;Database=...;User Id=...;Password=...;"`
- Definir variável de ambiente persistente (Windows): `setx TRILHA_CONNECTION "Server=...;Database=...;User Id=...;Password=...;"`
- A documentação interativa (Swagger) estará disponível em `http://localhost:5000` (ou porta indicada no terminal) e fornece endpoints para testes e para uso em Power BI / Tableau.

### Endpoints úteis para análise
- `GET /api/analytics/status-counts` — retorna contagem de tarefas por status
- `GET /api/analytics/tasks-per-day?start=YYYY-MM-DD&end=YYYY-MM-DD` — retorna número de tarefas por dia no intervalo
- `GET /api/analytics/summary` — resumo rápido (total, por status, últimos 30 dias)

### Consumir no Power BI / Tableau
- Power BI: use "Obter Dados -> Web" apontando para os endpoints acima, ou implemente autenticação se necessário.
- Tableau: use "Web Data Connector" apontando para os endpoints JSON.

### Extensões recomendadas (VS Code)
- `ms-dotnettools.csharp` — C# language support ✅
- `ms-mssql.mssql` — SQL Server support (querying/local management) ✅

---

> Nota: o projeto agora tem CORS habilitado (`AllowBI`) e endpoints de análise para facilitar integração com ferramentas de visualização.