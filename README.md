# CRN Technosoft - Product API (Technical Assessment)

## Tech Stack
- **.NET 8** / **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **JWT Authentication** with refresh token strategy
- **FluentValidation** for input validation
- **Swagger / OpenAPI** documentation
- **xUnit + Moq** for unit tests
- **Repository Pattern** + **Service Layer**
- **Clean Architecture** (Domain → Application → Infrastructure → API)

---

## Project Structure

```
ProductApi/
├── src/
│   ├── ProductApi.Domain/          # Entities, Exceptions
│   ├── ProductApi.Application/     # DTOs, Interfaces, Services, Validators
│   ├── ProductApi.Infrastructure/  # DbContext, Repositories, JWT TokenService
│   └── ProductApi.API/             # Controllers, Middleware, Program.cs
└── tests/
    └── ProductApi.Tests/           # Unit tests (xUnit + Moq)
```

---

## Setup Instructions

### 1. Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server / SQL Server Express

### 2. Update Connection String
Edit `src/ProductApi.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ProductApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply Database Migrations
Open **Package Manager Console** in Visual Studio, set default project to `ProductApi.Infrastructure`, then run:
```
Add-Migration InitialCreate
Update-Database
```

Or via CLI from the `src/ProductApi.API` folder:
```bash
dotnet ef migrations add InitialCreate --project ../ProductApi.Infrastructure
dotnet ef database update --project ../ProductApi.Infrastructure
```

### 4. Run the Project
Press **F5** in Visual Studio or:
```bash
cd src/ProductApi.API
dotnet run
```

Swagger UI: `https://localhost:{port}/swagger`

---

## API Endpoints

### Auth
| Method | URL | Description | Auth |
|--------|-----|-------------|------|
| POST | `/api/v1/auth/login` | Get JWT token | No |

### Products
| Method | URL | Description | Auth |
|--------|-----|-------------|------|
| GET | `/api/v1/products` | Get all products (paginated) | No |
| GET | `/api/v1/products/{id}` | Get product by Id | No |
| GET | `/api/v1/products/{id}/items` | Get items for a product | No |
| POST | `/api/v1/products` | Create product | ✅ JWT |
| PUT | `/api/v1/products/{id}` | Update product | ✅ JWT |
| DELETE | `/api/v1/products/{id}` | Delete product | ✅ JWT |

### Items
| Method | URL | Description | Auth |
|--------|-----|-------------|------|
| POST | `/api/v1/items` | Add item to a product | ✅ JWT |

---

## How to Use JWT in Swagger
1. Call `POST /api/v1/auth/login` with any username & password
2. Copy the token from the response
3. Click **Authorize** button in Swagger UI
4. Enter: `Bearer {your_token}`
5. Now you can call protected endpoints

---

## Database Schema (matches assessment)

```sql
CREATE TABLE [dbo].[Product] (
    [Id]           INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ProductName]  NVARCHAR(255) NOT NULL,
    [CreatedBy]    NVARCHAR(100) NOT NULL,
    [CreatedOn]    DATETIME NOT NULL,
    [ModifiedBy]   NVARCHAR(100) NULL,
    [ModifiedOn]   DATETIME NULL
)

CREATE TABLE [dbo].[Item] (
    [Id]           INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ProductId]    INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    [Quantity]     INT NOT NULL
)
```

---

## Running Tests
```bash
cd tests/ProductApi.Tests
dotnet test
```
