# AbInbev Challenge - Employee Management API

Este projeto é uma API RESTful construída em .NET 8 com intuito de gerenciar funcionários de uma empresa fictícia. Inclui autenticação via JWT, CRUD completo de funcionários, hierarquia de cargos (Employee, Manager, Director) e validações de negócio robustas.

A arquitetura segue os princípios de **Domain-Driven Design (DDD)**, organizando o código em camadas:

- **Domain:** Entidades (`Employee`, `PhoneNumber`, `Role`) e lógica de negócio.
- **Infrastructure:** Acesso a dados com EF Core, repositórios e configuração de `DbContext`.
- **Application:** Serviços, DTOs, validações e mapeamentos (métodos de extensão).
- **API (Presentation):** Controllers que expõem endpoints e orquestram serviços.

---

## Tecnologias e Padrões Utilizados

- **C# & .NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **Docker & Docker Compose** (para banco e API)
- **JWT (JSON Web Tokens)** para autenticação
- **Dependency Injection** nativo do ASP.NET Core
- **NUnit & Moq** para testes unitários
- **Design Patterns:** Repository, Service Layer, DTO
- **Logging:** `ILogger` integrado

---

## Endpoints

### Auth

| Método | Rota                       | Descrição                                                    |
|--------|----------------------------|--------------------------------------------------------------|
| POST   | `/api/auth/login`          | Autentica usuário existente e retorna token JWT.            |
| POST   | `/api/auth/register-director` | Registra o primeiro `Director` do sistema e retorna token.   |

### Employees (Protegido)

| Método | Rota                       | Descrição                                                    |
|--------|----------------------------|--------------------------------------------------------------|
| GET    | `/api/employees`           | Lista todos os funcionários.                                 |
| GET    | `/api/employees/{id}`      | Obtém detalhes de um funcionário por `id`.                   |
| POST   | `/api/employees`           | Cria um novo funcionário (Role ≤ criador).                   |
| PUT    | `/api/employees/{id}`      | Atualiza dados de um funcionário existente.                  |
| DELETE | `/api/employees/{id}`      | Remove um funcionário.                                       |

> **Observação:** O header `Authorization: Bearer {token}` deve ser enviado em todas as requisições protegidas.

---

## Executando com Docker Compose

Execute na raiz do projeto:

```bash
docker-compose up -d
```

Isso criará dois serviços:

- **db:** SQL Server 2022 em `localhost:1433`
- **api:** API exposta em `localhost:8090`

Verifique logs com:

```bash
docker-compose logs -f api
```

---

## Migrations

Para aplicar migrações e criar o banco de dados:

```bash
cd src/EmployeeManager.Infrastructure.Data
dotnet ef database update --context ApplicationDbContext
```

---

## Testes Unitários

Na raiz do projeto, execute:

```bash
dotnet test
```

O projeto utiliza **NUnit** e **Moq** para cobertura dos serviços de autenticação e gerenciamento de funcionários.

---


