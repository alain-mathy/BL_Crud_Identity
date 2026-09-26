# BL_Crud_Identity (Part 1)

A clean, structured, and production-ready **Blazor Web App** tutorial implementing a full-stack User Management system (CRUD) using **InteractiveAuto render mode** and **ASP.NET Core Identity**.

This repository is built following strict software engineering principles, decoupling data structures from the user interface using a **Shared** layer, **DTOs**, and **Interfaces**.

## 🏗️ Architecture Overview

The solution is divided into 3 distinct layers:
1. **BL_Crud_Identity.Shared**: Contains shared business models (`ApplicationUser`), Data Transfer Objects (`LoginDto`, `RegisterDto`, etc.), and service abstractions (`IUserService`).
2. **BL_Crud_Identity (Server)**: Implements database operations using Entity Framework Core, manages user authentication pipelines via Identity natively, and exposes secure API REST endpoints.
3. **BL_Crud_Identity.Client (WebAssembly)**: Handles interactive components rendering dynamically inside the browser, leveraging a custom HTTP communication channel with security state tracking.

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or higher
- Visual Studio 2026

### Database Initialization
Before running the application for the first time, open the **Package Manager Console** in Visual Studio and apply the database migrations to generate your SQLite/SQL Server structure:

```powershell
Update-Database
```

## ⏱️ Tutorial Git Commit Journey

This repository was created step-by-step using atomic commits to provide a clear learning path. You can review the git history to see the progression:

- `docs(tuto): Extend ApplicationUser with profile and address properties`
- `docs(tuto): Add Auth/User DTOs`
- `docs(tuto): Add IUserService`
- `docs(tuto): Add UserService`
- `docs(tuto): Add UsersController`
- `docs(tuto): Implement ClientUserService using HttpClient`
- `docs(tuto): `
  ```text
  Program.cs (Server)
  Add DI
  Configure HttpClient
  Modify AddIdentityCore -> options.SignIn.RequireConfirmedAccount = false;
  Add builder.Services.AddControllers();
  Add app.UseAuthentication(); and app.UseAuthorization(); and app.MapControllers();
  ```
- `docs(tuto):`
  ```text
  Add CookieHandler -> Client
  Add Register/Login pages
  Program.cs (Client): Add CookieHandler, HttpClient, DI
  Add Password policies and Lockout settings
  Program.cs (Server): Add builder.Services.AddAuthentication
  ```
- `docs(tuto):`
  ```text
  Modify Login page
  Add UsersList, EditUser pages
  Modify Program.cs (Server) -> move app.UseAntiforgery();
  ```
- `docs(tuto):`
  ```text
  Add Logout -> UsersController
  Add HomeCrud, CustomNavMenu pages
  Adapt MainLayout page
  ```
---

## 🔮 Roadmap for Part 2: Role-Based Access Control (RBAC)
In the next section of this course, we will scale this project to support enterprise access controls:
- Seed default **Admin** and **User** roles into the database on startup.
- Update `RegisterDto` or assign default fallback roles to newly created users.
- Enforce strict route protection using `@attribute [Authorize(Roles = "Admin")]` on `UsersList`.
- Hide UI elements conditionally using `<AuthorizeView Roles="Admin">`.
