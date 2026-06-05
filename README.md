# 🌵 Akaru API — .NET

API REST de gestão de dados do projeto **Akaru** (FIAP Global Solution 2026/1).

**Integrante:** Juan Pablo Rebelo Coelho  
**Stack:** ASP.NET Core 8 · Clean Architecture · Oracle EF Core · Firebase Admin SDK

## Responsabilidades

- Sincronização de usuários autenticados via **Firebase**
- CRUD de **plantios** do agricultor
- **Histórico** de recomendações vindas da API Java

## Arquitetura

```
src/
├── Akaru.Domain/          # Entidades e exceções
├── Akaru.Application/     # Serviços, DTOs, interfaces
├── Akaru.Infrastructure/  # EF Core Oracle, Firebase, repositórios
└── Akaru.API/             # Controllers, middleware, Swagger
tests/
└── Akaru.Tests/           # Testes xUnit (PlantioService)
```

Diagrama detalhado em [`docs/architecture.md`](docs/architecture.md).

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Oracle Database (XE ou cloud) com schema `AKARU`
- Projeto Firebase com Service Account (JSON)

## Configuração

1. Clone o repositório
2. Execute o DDL em `scripts/script-bd.sql` no Oracle
3. Copie `appsettings.Development.json.example` para `appsettings.Development.json`
4. Coloque o JSON do Firebase em `src/Akaru.API/firebase-credentials.json`
5. Ajuste a connection string Oracle

```json
{
  "ConnectionStrings": {
    "Oracle": "User Id=akaru;Password=SUA_SENHA;Data Source=HOST:1521/XEPDB1"
  },
  "Firebase": {
    "ProjectId": "seu-projeto",
    "CredentialsPath": "firebase-credentials.json",
    "UseMockAuth": false
  }
}
```

## Executar

```bash
cd src/Akaru.API
dotnet run
```

Swagger: http://localhost:5001/swagger  
Health: http://localhost:5001/health

## Autenticação

Todas as rotas protegidas exigem:

```
Authorization: Bearer <firebase_id_token>
```

### Fluxo mobile

1. Login/cadastro no Firebase (app React Native)
2. `POST /api/usuarios/sync` — cria usuário no Oracle
3. Demais chamadas com o mesmo token

## Exemplos de requests

### Sync usuário

```http
POST /api/usuarios/sync
Authorization: Bearer {firebase_token}
```

**Response 200:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "latitude": null,
  "longitude": null,
  "cidade": null,
  "estado": null,
  "dataCadastro": "2026-06-01T14:30:00Z"
}
```

### Registrar plantio

```http
POST /api/plantios
Authorization: Bearer {firebase_token}
Content-Type: application/json

{
  "culturaId": 3,
  "latitude": -23.5505,
  "longitude": -46.6333,
  "dataPlantio": "2026-06-15T00:00:00Z",
  "detalhes": "Solo argiloso, área de 2 hectares",
  "cidade": "São Paulo",
  "estado": "SP"
}
```

### Salvar no histórico

```http
POST /api/historico
Authorization: Bearer {firebase_token}
Content-Type: application/json

{
  "culturaId": 3,
  "culturaNome": "Milho",
  "textoRecomendacao": "Plante entre setembro e novembro...",
  "score": 87.5,
  "latitude": -23.5505,
  "longitude": -46.6333,
  "detalhes": "Área irrigada"
}
```

## Testes

```bash
dotnet test
```

## Migrations EF Core

```bash
cd src/Akaru.API
dotnet ef migrations add InitialCreate --project ../Akaru.Infrastructure
dotnet ef database update --project ../Akaru.Infrastructure
```

## CORS

Configurado para aceitar requisições do app mobile (`AllowAnyOrigin` em desenvolvimento).

## Equipe Akaru

| Integrante | Papel |
|------------|-------|
| Juan Pablo Rebelo Coelho | API .NET (este repo) |
| Victor | API Java + Gemini |
| Luann | App React Native |
| Lucas Higuti | Oracle PL/SQL |
| Renato | Arquitetura + IA |
