# Sistema de Controle Financeiro

Sistema web full-stack para gerenciamento financeiro desenvolvido com .NET 8 (backend) e React + TypeScript (frontend).

## 🚀 Tecnologias

**Backend:**
- .NET 8 / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- MediatR (CQRS)
- FluentValidation
- Docker

**Frontend:**
- React 18 + TypeScript
- Vite
- Shadcn UI
- Tailwind CSS

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** e **DDD**, organizado em camadas:

- **API**: Controllers, DTOs, Middleware
- **Application**: Commands, Queries, Handlers (CQRS)
- **Domain**: Entities, Value Objects, Repository Interfaces
- **Infrastructure**: DbContext, Repositories, Mappings

## 🛠️ Execução

Execute o projeto com Docker Compose:

```bash
docker compose up --build
```

Acesse:
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger

## 📋 Funcionalidades

- Cadastro de pessoas (com validação de CPF)
- Cadastro de categorias
- Registro de transações financeiras
- Relatórios e consultas de totais
