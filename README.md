# Minimal API - Gestão de Veículos

API construída com .NET 7 usando o modelo Minimal API, com autenticação JWT, CRUD de veículos e testes automatizados.

## Tecnologias

- .NET 7
- ASP.NET Core Minimal APIs
- Entity Framework Core 7
- MySQL (Pomelo)
- JWT Bearer
- Swagger
- MSTest

## Como executar

### 1. Subir o MySQL

```bash
docker compose up -d
```

O banco `minimal_api` será criado em `localhost:3306` com usuário `root` e senha `root`.

### 2. Aplicar as migrations

```bash
dotnet ef database update --project Api/mininal-api.csproj
```

> Se preferir, use um MySQL já existente e ajuste a connection string em `Api/appsettings.json`.

### 3. Rodar a API

```bash
dotnet run --project Api/mininal-api.csproj
```

A documentação Swagger fica disponível em `https://localhost:5001/swagger` (ou na porta configurada em `Api/Properties/launchSettings.json`).

## Autenticação

A migration de seed cria o administrador:

- **E-mail:** `administrador@teste.com`
- **Senha:** `123456`

Faça login para obter o token:

```bash
curl -X POST http://localhost:5000/administradores/login \
  -H "Content-Type: application/json" \
  -d '{"email":"administrador@teste.com","senha":"123456"}'
```

Envie o token nas demais requisições:

```
Authorization: Bearer <token>
```

## Endpoints

### Administradores

| Método | Rota | Descrição | Perfil |
| --- | --- | --- | --- |
| POST | `/administradores/login` | Autentica e retorna o token JWT | Público |
| GET | `/administradores?pagina=1` | Lista administradores paginados | Adm |
| GET | `/administradores/{id}` | Busca administrador por ID | Adm |
| POST | `/administradores` | Cadastra administrador | Adm |

### Veículos

| Método | Rota | Descrição | Perfil |
| --- | --- | --- | --- |
| GET | `/veiculos?pagina=1&nome=&marca=` | Lista veículos com filtros e paginação | Autenticado |
| GET | `/veiculos/{id}` | Busca veículo por ID | Adm/Editor |
| POST | `/veiculos` | Cadastra veículo | Adm/Editor |
| PUT | `/veiculos/{id}` | Atualiza veículo | Adm |
| DELETE | `/veiculos/{id}` | Remove veículo | Adm |

## Testes

```bash
dotnet test Test/Test.csproj
```

Os testes de integração usam um mock do serviço de administradores. Os testes de persistência exigem o MySQL em execução.
