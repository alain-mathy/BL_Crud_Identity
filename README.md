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

## 🔐 Part 2: Role-Based Access Control (RBAC) & Data Ownership

This section scales the architecture to support enterprise-grade security policies, automatically segregation roles, and strictly validation database resource access rights at the server perimeter.

### ⏱️ Step-by-Step Part 2 Git Journey

- `feat(tuto-p2): Add automatic database seeding for default Admin/User roles and master account`
  ```text
  Add IdentityDataInitializer
  Adapt Program.cs (Server):
    .AddRoles<IdentityRole>() to AddIdentityCore
  see just before app.run() -> using (var scope = app.Services.CreateScope())
  ```
- `feat(tuto-p2): Assign default User role inside the strict IUserService Register workflow`
  ```text
  Add Role -> UserDto
  Modify UserService -> RegisterAync -> add default role User
  Modify UserService for roles -> GetAllUsersAsync, GetUserByIdAsync
  Modify UsersList -> add Role
  ```
- `feat(tuto-p2): update UsersList grid table view UI to display dynamic role badges`
  ```text
    Add Role -> UserDto
    Modify UserService -> RegisterAync -> add default role User
    Modify UserService for roles -> GetAllUsersAsync, GetUserByIdAsync
    Modify UsersList -> add Role
  ```
- `feat(tuto-p2): Restructure Identity Views & Navigation Profiles`
  ```text
  Adapt UsersList depending on Admin/User/Not connected
  Adapt CustomNavMenu depending on Admin/User
  ```
- `secure(tuto-p2): Core API Controller Hardening`
  ```text
  Secure UsersController
  ```

---

## 🏗️ Core Security Mechanisms Implemented in Part 2

1. **Automated Seeding System:** The application safely generates essential system roles (`Admin`, `User`) and a master power account (`admin@admin.com`) invisibly on the very first execution lifecycle.
2. **Dynamic UI Shifting:** Regular users are completely sandboxed away from the administration layout grid. Selecting the navigation option transparently routes their session straight to their specific personal data sheet.
3. **Double-Gate Server Validation:** URL obfuscation or structural JSON payload manipulation is instantly stopped at the API Controller layer. Request tokens are continuously validated against active session Claims, systematically returning an `HTTP 403 Forbidden` response header upon unauthorized cross-profile interactions.

