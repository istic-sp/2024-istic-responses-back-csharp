# ISTIC.Responses

O pacote **ISTIC.Responses** foi desenvolvido pelo Instituto SENAI de Tecnologia da Informação e Comunicação (ISTIC) para fornecer uma base padronizada de respostas para endpoints de APIs em C# .NET.

## Estrutura do Projeto

```
├── ISTIC.Responses/                  # Pacote (biblioteca) de respostas padronizadas (.NET 8)
│   ├── Core/                         # Classes principais
│   │   ├── Response.cs               # Resposta sem corpo (sucesso vazio ou erro padrão)
│   │   ├── ResponseOf<T>.cs          # Resposta com resultado genérico ou erro padrão
│   │   ├── Error.cs                  # Modelo de erro padrão
│   │   ├── CustomError<T>.cs         # Modelo de erro com campo Data genérico
│   │   ├── CustomResponse<T>.cs      # Resposta sem corpo com erro customizado
│   │   └── CustomResponseOf<T,E>.cs  # Resposta com resultado genérico ou erro customizado
│   ├── Converters/                   # Conversores JSON (System.Text.Json)
│   ├── Extensions/                   # ErrorFactory, ResponseExtensions, SystemTypeExtensions
│   ├── Filters/                      # CustomActionFilter (define o status code da resposta HTTP)
│   ├── Swagger/                      # ResponseOperationFilter (documenta schemas de sucesso/erro)
│   └── Interfaces/                   # IResponse
│
└── ISTIC.Responses.WebApi/           # API de demonstração (.NET 10)
    ├── Program.cs                    # Configuração da aplicação
    ├── DTOs/                         # Modelos de request e result
    └── Features/Responses/           # Controller e métodos de exemplo (CRUD de Produtos)
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (necessário para o projeto WebApi)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (necessário para o projeto do pacote)

## Como executar

```bash
# Clone o repositório
git clone https://github.com/istic-sp/2024-istic-responses-back-csharp.git
cd 2024-istic-responses-back-csharp

# Restaure as dependências
dotnet restore

# Execute a API de demonstração
dotnet run --project ISTIC.Responses.WebApi
```

Após executar, acesse o Swagger UI em: **https://localhost:{porta}/swagger**

---

## Tipos de Resposta

O pacote oferece três tipos de resposta para cobrir diferentes cenários:

### 1. `Response` — Resposta sem corpo

Para endpoints que não precisam retornar dados no sucesso (ex.: DELETE, PUT).

- **Sucesso:** retorna apenas o status code (sem corpo)
- **Erro:** retorna o objeto `Error` padrão

```csharp
public async Task<Response> Delete(Guid id)
{
    // Sucesso (200)
    return Response.Success();

    // Sucesso com status code diferente
    return Response.Success(HttpStatusCode.NoContent);

    // Erro
    return Response.ErrorHandle("Error", "Usuário não encontrado.", HttpStatusCode.NotFound);
}
```

### 2. `ResponseOf<T>` — Resposta com resultado genérico

Para endpoints que retornam dados no sucesso (ex.: GET, POST).

- **Sucesso:** retorna o objeto `T` como corpo da resposta
- **Erro:** retorna o objeto `Error` padrão

```csharp
public async Task<ResponseOf<RegisterResult<Guid>>> Add(Model request)
{
    // Sucesso — conversão implícita
    return new RegisterResult<Guid> { Id = Guid.NewGuid() };

    // Erro — via ErrorFactory
    return ErrorFactory.BadRequestError("Erro de validação.");
}
```

### 3. `CustomResponseOf<TResult, TError>` — Resposta com erro customizado

Para endpoints que precisam retornar **dados adicionais** no erro, além do `Error` padrão.

- **Sucesso:** retorna o objeto `TResult` como corpo da resposta
- **Erro:** retorna o objeto `CustomError<TError>`, que herda de `Error` e adiciona um campo `Data` do tipo `TError`

```csharp
public async Task<CustomResponseOf<RegisterResult<Guid>, CustomErrorResult>> Create(CreateProductRequest request)
{
    // Sucesso — conversão implícita
    return new RegisterResult<Guid> { Id = Guid.NewGuid() };

    // Erro com dados customizados
    return ErrorFactory.CustomError(
        "Já existe um produto com este nome.",
        HttpStatusCode.Conflict,
        null,
        new CustomErrorResult("Conflito", "Já existe um produto com este nome.")
    );

    // Erro sem dados customizados (Data será null)
    return ErrorFactory.CustomErrorWithoutData<CustomErrorResult>(
        "Erro de validação.",
        HttpStatusCode.BadRequest,
        fieldErrors
    );
}
```

### Modelo de erro padrão (`Error`)

```json
{
  "name": "Bad Request",
  "description": "Erro de validação ao criar o produto.",
  "fieldErrors": {
    "name": ["O campo Nome é obrigatório."]
  }
}
```

### Modelo de erro customizado (`CustomError<T>`)

```json
{
  "name": "Conflict",
  "description": "Já existe um produto com este nome.",
  "fieldErrors": {},
  "data": {
    "title": "Conflito",
    "description": "Já existe um produto com este nome."
  }
}
```

---

## Como configurar em seu projeto

### 1. Adicionar filtros e conversores JSON

No `Program.cs` (ou `Startup.cs`):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomActionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ResponseOfJsonConverterFactory());
    options.JsonSerializerOptions.Converters.Add(new ResponseJsonConverterFactory());
    options.JsonSerializerOptions.Converters.Add(new CustomResponseOfJsonConverterFactory());
});
```

### 2. Configurar o Swagger

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(d => d.GetSchemaId());
    options.OperationFilter<ResponseOperationFilter>();
});
```

### 3. ErrorFactory

Utilize a classe `ErrorFactory` para criar erros padronizados de forma rápida:

| Método | Status Code |
|---|---|
| `ErrorFactory.BadRequestError()` | 400 |
| `ErrorFactory.UnauthorizedError()` | 401 |
| `ErrorFactory.ForbiddenError()` | 403 |
| `ErrorFactory.NotFoundError()` | 404 |
| `ErrorFactory.InternalServerError()` | 500 |
| `ErrorFactory.CustomError(description, statusCode)` | Qualquer |
| `ErrorFactory.CustomError<T>(description, statusCode, fieldErrors, data)` | Qualquer (com dados customizados) |
| `ErrorFactory.CustomErrorWithoutData<T>(description, statusCode, fieldErrors)` | Qualquer (sem dados customizados) |

### 4. Status code customizado em respostas de sucesso

```csharp
return new RegisterResult<Guid> { Id = Guid.NewGuid() }
    .WithSuccessStatusCode(HttpStatusCode.Created); // 201
```

---

## Como testar (Endpoints da API de demonstração)

A API de demonstração (`ISTIC.Responses.WebApi`) expõe um CRUD de Produtos no controller `api/Response`. Todos os endpoints simulam cenários de sucesso e erro através de parâmetros de query string.

### GET `api/Response` — Listar produtos

Retorno: `ResponseOf<ProductRequest>`

| Cenário | Como testar |
|---|---|
| ✅ Sucesso (200) | `GET /api/Response` |
| ❌ InternalServerError (500) | `GET /api/Response?simulateError=true` |

### GET `api/Response/{id}` — Buscar produto por Id

Retorno: `ResponseOf<ProductRequest>`

| Cenário | Como testar |
|---|---|
| ✅ Sucesso (200) | `GET /api/Response/3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| ❌ BadRequest (400) | `GET /api/Response/00000000-0000-0000-0000-000000000000` |
| ❌ Unauthorized (401) | `GET /api/Response/3fa85f64-5717-4562-b3fc-2c963f66afa6?simulateUnauthorized=true` |

### POST `api/Response` — Criar produto

Retorno: `CustomResponseOf<RegisterResult<Guid>, CustomErrorResult>`

| Cenário | Como testar |
|---|---|
| ✅ Sucesso (200) | Enviar body com `name` preenchido e `price` > 0 |
| ❌ BadRequest (400) — Nome vazio | Enviar body com `name` vazio (`""`) |
| ❌ BadRequest (400) — Preço inválido | Enviar body com `price` = 0 ou negativo |
| ❌ Conflict (409) — Erro customizado | `POST /api/Response?simulateCustomError=true` com body válido |

**Body de exemplo (sucesso):**
```json
{
  "name": "Notebook",
  "description": "Notebook Dell Inspiron",
  "price": 4500.00
}
```

**Body de exemplo (erro de validação — nome vazio):**
```json
{
  "name": "",
  "description": "Descrição",
  "price": 100.00
}
```

### PUT `api/Response/{id}` — Atualizar produto

Retorno: `Response`

| Cenário | Como testar |
|---|---|
| ✅ Sucesso (200) | `PUT /api/Response/00000000-0000-0000-0000-000000000001` com body válido |
| ❌ BadRequest (400) — Id vazio | `PUT /api/Response/00000000-0000-0000-0000-000000000000` |
| ❌ BadRequest (400) — Nome vazio | Enviar body com `name` vazio |
| ❌ NotFound (404) | `PUT /api/Response/3fa85f64-5717-4562-b3fc-2c963f66afa6` (qualquer Id diferente de `000...001`) |
| ❌ Forbidden (403) | `PUT /api/Response/00000000-0000-0000-0000-000000000001?simulateForbidden=true` |

**Body de exemplo:**
```json
{
  "name": "Notebook Atualizado",
  "description": "Notebook Dell Inspiron 15",
  "price": 5000.00
}
```

### DELETE `api/Response/{id}` — Deletar produto

Retorno: `Response`

| Cenário | Como testar |
|---|---|
| ✅ Sucesso (200) | `DELETE /api/Response/00000000-0000-0000-0000-000000000001` |
| ❌ BadRequest (400) — Id vazio | `DELETE /api/Response/00000000-0000-0000-0000-000000000000` |
| ❌ NotFound (404) | `DELETE /api/Response/3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| ❌ InternalServerError (500) | `DELETE /api/Response/3fa85f64-5717-4562-b3fc-2c963f66afa6?simulateInternalError=true` |

---

## Observações importantes

- Sempre utilize `ResponseOf<T>`, `CustomResponseOf<TResult, TError>` ou `Response` como tipo de retorno dos endpoints para que os filtros do Swagger e o `CustomActionFilter` funcionem corretamente.
- O `CustomActionFilter` é responsável por definir o status code HTTP da resposta baseado na propriedade `StatusCode` da `IResponse`.
- Os conversores JSON (`ResponseOfJsonConverterFactory`, `ResponseJsonConverterFactory`, `CustomResponseOfJsonConverterFactory`) garantem que apenas o `Result` (sucesso) ou o `Error`/`CustomError` (erro) seja serializado no corpo da resposta — nunca ambos ao mesmo tempo.