# 📋 Tasks Backend API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](#)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql&logoColor=white)](#)
[![MediatR](https://img.shields.io/badge/MediatR-CQRS-blue)](#)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-success)](#)

API RESTful para gerenciamento corporativo de tarefas, usuários e estrutura organizacional hierárquica. Desenvolvida em **.NET 10**, seguindo rigorosamente os princípios de **Clean Architecture** e o padrão **CQRS** com **MediatR**.

Esta API foi projetada para alimentar aplicações SPA modernas (como Vue.js) de forma segura, performática e escalável.

---

## ✨ Principais Funcionalidades

- 🔐 **Onboarding Seguro**: Pré-cadastro via e-mail com link de ativação seguro (SMTP) e posterior definição de senha.
- 🛡️ **Autenticação & Autorização**: ASP.NET Core Identity com tokens JWT Bearer (`HMAC-SHA256`).
- 🏢 **Hierarquia Organizacional**: Estrutura em árvore para gerenciar diretorias, departamentos e subsetores (`OrganizationUnit`).
- ✅ **Gestão de Tarefas (CRUD)**: Fluxo completo de criação, atribuição e acompanhamento de tarefas.
- 🚦 **Validação Global**: Pipeline do MediatR configurado com `FluentValidation` (Fail-Fast).
- 🐛 **Tratamento Global de Erros**: Respostas de erro padronizadas no formato **RFC 7807** (`ProblemDetails`).
- 🗑️ **Soft Delete Automático**: Entidades não são excluídas fisicamente, utilizando *Global Query Filters* nativos do EF Core.

---

## 🚀 Stack Tecnológica

- **Framework:** .NET 10 (C# 10) / ASP.NET Core Web API
- **Banco de Dados:** PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- **ORM:** Entity Framework Core 10
- **Arquitetura:** MediatR (CQRS) + FluentValidation
- **Identidade:** ASP.NET Core Identity EF Core
- **Mensageria:** MailKit / MimeKit (SMTP transacional)
- **Documentação API:** Scalar / OpenAPI
- **Testes:** xUnit + FluentAssertions + EF Core InMemory

---

## 🏛️ Arquitetura

O projeto adota a **Clean Architecture** combinada com **CQRS** (separação de responsabilidades de Leitura e Escrita), promovendo a **Localidade de Comportamento** (Primary Constructors, Records, e Commands/Handlers no mesmo arquivo).

```text
Src/
├── API/              # Controllers limpos, Middlewares (GlobalExceptionHandler)
├── Application/      # Casos de uso (Commands/Queries), Behaviors de Validação e DTOs
├── Domain/           # Entidades de negócio (TaskItem, OrganizationUnit) e classes base
└── Infrastructure/   # AppDbContext (PostgreSQL), Configurações do Identity e Serviços Externos (Email)