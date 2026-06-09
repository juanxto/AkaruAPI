# Arquitetura — Akaru API (.NET)

**Integrante:** Juan Pablo Rebelo Coelho  
**Disciplina:** Advanced Business Development with .NET  
**Projeto:** Akaru — FIAP Global Solution 2026/1

## Visão geral

A API .NET é responsável pela **gestão de dados persistentes** do Akaru:

- Cadastro e autenticação de usuários via **JWT**
- CRUD de **plantios** registrados pelo agricultor
- **Histórico** de recomendações geradas pela API Java (Gemini)

```mermaid
flowchart LR
    Mobile[App React Native] -->|JWT Bearer| API[Akaru.API]
    API --> App[Akaru.Application]
    App --> Infra[Akaru.Infrastructure]
    Infra --> Oracle[(Oracle DB)]
    Mobile --> Java[API Java - Recomendacoes]
```

## Clean Architecture

| Camada | Responsabilidade |
|--------|------------------|
| **Domain** | Entidades, exceções de domínio |
| **Application** | DTOs, interfaces, serviços (regras de negócio) |
| **Infrastructure** | EF Core Oracle, repositórios |
| **API** | Controllers, JWT, middleware, Swagger |

### SOLID aplicado

- **SRP:** cada serviço (`UsuarioService`, `PlantioService`, `HistoricoService`) tem uma responsabilidade
- **DIP:** serviços dependem de interfaces (`IPlantioRepository`, etc.), não de implementações concretas

## Autenticação (JWT)

O cliente obtém um token via cadastro ou login:

```
Authorization: Bearer <jwt_token>
```

Fluxo:

1. `POST /api/auth/register` ou `POST /api/auth/login` retorna JWT
2. `JwtBearer` middleware valida assinatura e expiração
3. Claim `sub` contém o ID do usuário no Oracle
4. Controllers usam `User.ObterUsuarioId()` para identificar o usuário

Senhas são armazenadas com **PBKDF2** (`PasswordHasher`). Usuários do seed GS com senha em texto plano continuam funcionando no login.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/register` | Cadastro + JWT |
| POST | `/api/auth/login` | Login + JWT |
| GET | `/api/usuarios/me` | Perfil do usuário logado |
| PUT | `/api/usuarios/me` | Atualiza perfil |
| POST | `/api/plantios` | Registra plantio |
| GET | `/api/plantios` | Lista plantios do usuário |
| GET | `/api/plantios/{id}` | Detalhe do plantio |
| PUT | `/api/plantios/{id}` | Atualiza plantio |
| DELETE | `/api/plantios/{id}` | Remove plantio |
| POST | `/api/historico` | Salva recomendação |
| GET | `/api/historico` | Lista histórico |
| GET | `/api/historico/{id}` | Detalhe da recomendação |

## Relacionamentos no banco

- **1:N** — `TB_USUARIO` → `TB_PLANTIO`
- **1:N** — `TB_USUARIO` → `TB_HISTORICO_RECOMENDACAO`
- **N:N** — `TB_PLANTIO` ↔ culturas via `TB_PLANTIO_CULTURA`

## Health Checks

- `GET /health` — verifica API + conexão Oracle via EF Core

## Integração com o mobile (Luann)

Base URL sugerida em desenvolvimento: `http://localhost:5001`

O app deve chamar `POST /api/auth/login` (ou `register`) e enviar o JWT em todas as requisições protegidas.
