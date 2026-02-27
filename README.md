# Distributed Deal Management System

### High-Performance Enterprise Backend built with .NET 10

This project is a robust, distributed ecosystem designed for managing business deals. It serves as a comprehensive demonstration of modern backend engineering, focusing on **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Event-Driven Microservices**.

---

## 🏗 Architectural Overview

The system is built using a layered approach (Clean Architecture) to ensure loose coupling, high maintainability, and testability:

- **Domain Layer**: Contains pure business entities (`Deal`), value objects, and core domain logic (State transition validation).
- **Application Layer**: Implements use cases via the **CQRS (Command Query Responsibility Segregation)** pattern using **MediatR**. 
- **Infrastructure Layer**: Handles external concerns such as data persistence (**EF Core + PostgreSQL**) and message brokering (**Apache Kafka**).
- **API Layer**: The entry point for external requests, managing routing, security (JWT), and documentation.

## 🛠 Tech Stack

- **Runtime**: .NET 10 (C#)
- **Web Framework**: ASP.NET Core Web API
- **Persistence**: PostgreSQL + Entity Framework Core (Migrations, Fluent API)
- **Messaging**: Apache Kafka (Confluent Schema-less Producer/Consumer pattern)
- **Security**: Keycloak (OAuth 2.0 / OpenID Connect / JWT)
- **Patterns**: MediatR (CQRS), Result Pattern, Global Exception Handling (Middleware), Action Filters
- **Docs & UI**: Scalar / OpenAPI (Swagger)
- **Containerization**: Docker Compose

---

## 🚀 Key Features

1. **State-Driven Business Logic**: Deal lifecycles (`New` -> `InProgress` -> `Closed`) are enforced within the Domain model, preventing invalid state transitions.
2. **Enterprise-Grade Security**: All endpoints are secured with JWT tokens issued by **Keycloak**. Includes fine-grained resource ownership checks.
3. **Asynchronous Communication**: Implements **Event-Driven Architecture**. Upon deal creation, the `DealService` publishes an integration event to Kafka.
4. **Resilient Background Processing**: A dedicated `NotificationService` (Worker Service) asynchronously consumes Kafka events to handle side effects without blocking the main API.
5. **Observability & Error Handling**: Centralized exception management via custom Middleware, providing standard **RFC 7807** Problem Details responses.
6. **Clean Code & SOLID**: Strict adherence to SOLID principles and the **Single Responsibility Principle** through MediatR Handlers.

---

## 📦 Getting Started

### 1. Spin up Infrastructure (Docker)
Ensure Docker Desktop is running. From the project root, execute:
```bash
docker-compose up -d