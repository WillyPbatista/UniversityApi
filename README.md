# UniversityApi
A .NET 8 Web API built following the Clean Architecture principles. The project is designed to demonstrate a modular, scalable, and maintainable architecture for building enterprise-grade APIs with Entity Framework Core and Domain-Driven Design concepts.

## 🚀 Features

- **ASP.NET Core 8 Web API**
- **Clean Architecture** project structure
- **Entity Framework Core** integration (Code-First)
- Separation of concerns: Domain, Application, Infrastructure, and API
- Ready for dependency injection and future extensions
- Example ready for **SQL Server**, but easily configurable for other databases

---

## 🏗️ Architecture Overview

UniversityApi/
├── University.Api/ → Presentation layer (Controllers, Startup)
├── University.Application/ → Business logic, use cases, DTOs
├── University.Domain/ → Entities, Enums, Value Objects, Interfaces
├── University.Infrastructure/ → Database access, EF Core context, repositories
└── UniversityApi.sln → Solution file

yaml

Each layer has a **clear responsibility** and communicates only with the layer directly below it.  
This ensures high cohesion and low coupling — the essence of Clean Architecture.

---

## ⚙️ Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Swagger / OpenAPI](https://swagger.io/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (default)
- C# 12

---

## 🧩 Getting Started

### 1️⃣ Clone the repository

git clone https://github.com/<your-username>/UniversityApi.git
cd UniversityApi
2️⃣ Build the solution

dotnet build
3️⃣ Run the API

cd University.Api
dotnet run
The API will start by default at
https://localhost:5001 or http://localhost:5000

Database Setup (EF Core)
To add Entity Framework Core and create your database, run the following commands:

cd University.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
Then, create your DbContext (e.g., UniversityDbContext.cs) and run migrations:

dotnet ef migrations add InitialCreate -p University.Infrastructure -s University.Api
dotnet ef database update -p University.Infrastructure -s University.Api
Project Dependencies
Project	Depends On
University.Api	University.Application, University.Infrastructure
University.Application	University.Domain
University.Infrastructure	University.Domain
University.Domain	(no dependencies)

Author
William Javier Perez Batista
Engineer in Computer Science — passionate about software architecture, backend development, and scalable design.
