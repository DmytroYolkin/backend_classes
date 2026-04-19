# Agents.md — Retail Store API

## Overview

This document describes the logical “agents” (components/layers) in the Retail Store API and how they interact.

The system is built using:

* ASP.NET Core Minimal API
* GraphQL (Hot Chocolate)
* Entity Framework Core (PostgreSQL)
* FluentValidation

---

## 🧩 Core Agents

### 1. API Layer (Entry Points)

#### Minimal API Agent

Handles REST endpoints under `/api/*`.

Responsibilities:

* Accept HTTP requests
* Validate input (via FluentValidation)
* Call service layer
* Return HTTP responses (200, 400, 404)

Example:

```
GET /api/products
POST /api/products
```

---

#### GraphQL Agent

Handles flexible queries via `/graphql`.

Responsibilities:

* Accept GraphQL queries/mutations
* Support filtering, sorting, projections
* Call service layer
* Return structured GraphQL responses

---

## 🧠 2. Service Layer (Business Logic Agent)

Acts as the brain of the system.

Responsibilities:

* Business rules
* Validation coordination
* Data transformation
* Calling repositories

Examples:

* Ensure product price > 0
* Ensure category exists before creating product
* Apply discount logic

---

## 🗄️ 3. Repository Layer (Data Access Agent)

Responsible for communication with the database.

Responsibilities:

* Query database via DbContext
* Include navigation properties
* Return entities

Example:

```csharp
_context.Products
    .Include(p => p.Category)
    .Include(p => p.Discount)
```

---

## 🧱 4. DbContext (Persistence Agent)

Central EF Core component.

Responsibilities:

* Database connection
* Entity tracking
* Migrations
* Seeding data

Contains:

* DbSets (Products, Categories, Discounts, Employees)
* Fluent API configurations
* Seed data via `HasData`

---

## 📦 5. Domain Entities (Data Model Agent)

Represents database structure.

Entities:

* Product
* Category
* Discount
* Employee

Responsibilities:

* Define relationships
* Represent tables

---

## 🔄 6. DTOs (Data Transfer Agent)

Used for communication between API and clients.

Responsibilities:

* Shape request/response data
* Hide internal fields
* Prevent overexposure

Example:

```
ProductDto
CreateProductDto
UpdateProductDto
```

---

## 🔁 7. Mapping Agent

Converts between DTOs and Entities.

Responsibilities:

* DTO → Entity (incoming data)
* Entity → DTO (outgoing data)

Can be:

* Manual mapping
* AutoMapper (optional)

---

## ✅ 8. Validation Agent (FluentValidation)

Ensures correctness of input data.

Responsibilities:

* Enforce rules
* Return structured errors

Examples:

* Price > 0
* Stock ≥ 0
* Discount percentage between 1–100

---

## 🔗 Data Flow

### REST Flow

```
Client → Minimal API → Validation → Service → Repository → DbContext → Database
                                                        ↓
                                                   Entity → DTO → Response
```

### GraphQL Flow

```
Client → GraphQL → Resolver → Service → Repository → DbContext → Database
                                                     ↓
                                                Entity → GraphQL Response
```

---

## 📁 Suggested Folder Structure

```
API
│
├── /Controllers        (optional, if extended)
├── /Endpoints          (Minimal API route groups)
├── /GraphQL
│   ├── Query.cs
│   ├── Mutation.cs
│
├── /Services
├── /Repositories
├── /Data
│   ├── AppDbContext.cs
│
├── /Entities
├── /DTOs
├── /Validators
├── /Mappings
|
Tests

```

---

## ⚠️ Design Rules

* Do NOT expose Entities directly in API
* Always validate input before saving
* Keep DbContext only in Data layer
* Services should not depend on API layer
* Repositories should not contain business logic

---

## 🧪 Testing Agent

Using `.http` file and XUnit.

Responsibilities:

* Validate endpoints
* Ensure database operations work
* Test GraphQL queries and mutations

---

## 🧠 Summary

| Agent      | Responsibility       |
| ---------- | -------------------- |
| API        | Entry point          |
| GraphQL    | Flexible querying    |
| Service    | Business logic       |
| Repository | Data access          |
| DbContext  | Database interaction |
| Entities   | Database structure   |
| DTOs       | Data transfer        |
| Mapping    | Transformation       |
| Validation | Input correctness    |

---

## Final Thought

Think of the system like a pipeline:

> Input → Validation → Logic → Data → Output

Each agent has one job. Keep them separated, and your system stays scalable and clean.
