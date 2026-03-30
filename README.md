# Quantity Measurement Application

Layered .NET 8 solution for quantity comparison, conversion, arithmetic, authentication, and persistence.

The project supports:

- Length, weight, volume, and temperature measurements
- Compare, convert, add, subtract, and divide operations
- Console app workflow and ASP.NET Core Web API
- JWT-based authentication for protected quantity endpoints
- Repository implementations for cache, ADO.NET SQL, and EF Core ORM
- MSTest coverage across domain and business scenarios

## Supported Measurement Types

- Length: `Feet`, `Inches`, `Yards`, `Centimeters`
- Weight: `Gram`, `Kilogram`, `Pound`, `Tonne`
- Volume: `Litre`, `Millilitre`, `Gallon`
- Temperature: `Celsius`, `Fahrenheit`, `Kelvin`

Temperature supports comparison and conversion. Arithmetic operations on temperature are blocked by domain rules.

## Full Repository Structure

```text
QuantityMeasurementApp.sln
README.md
QuantityMeasurementSolution/
|- QuantityMeasurementDb.sql
|- QuantityMeasurementSolution.slnx
|- BusinessLayer/
|  |- Interfaces/
|  |  |- IAuthService.cs
|  |  `- IQuantityMeasurementService.cs
|  `- Services/
|     |- AuthService.cs
|     `- QuantityMeasurementService.cs
|- ModelLayer/
|  |- DTOs/
|  |- Entities/
|  `- Models/
|- RepositoryLayer/
|  |- Configuration/
|  |- Data/
|  |- Interfaces/
|  |- Migrations/
|  `- Repositories/
|- QuantityMeasurement.Domain/
|  |- Enums/
|  |- Exceptions/
|  |- Services/
|  `- ValueObjects/
|- QuantityMeasurement.Api/
|  |- Contracts/
|  `- Controllers/
|- QuantityMeasurementApp/
|  |- Configuration/
|  `- Controllers/
`- QuantityMeasurement.Tests/
```

## Architecture (Project-by-Project)

1. `QuantityMeasurement.Domain`
	- Core business rules, units, value objects, conversion and arithmetic logic.
2. `ModelLayer`
	- Shared DTOs, entities, and models used across layers.
3. `RepositoryLayer`
	- Persistence and data access (`Cache`, `Database` with ADO.NET, `ORM` with EF Core), DbContext, and migrations.
4. `BusinessLayer`
	- Application services (`IQuantityMeasurementService`, `IAuthService`) orchestrating domain + repository calls.
5. `QuantityMeasurement.Api`
	- ASP.NET Core API with JWT auth, swagger, controllers, and request/response contracts.
6. `QuantityMeasurementApp`
	- Console client for interactive quantity operations.
7. `QuantityMeasurement.Tests`
	- MSTest suite for generic quantity logic, advanced operations, UC scenarios, and service behavior.

## Solutions in This Repository

- `QuantityMeasurementApp.sln`
  - Main Visual Studio solution that includes all layer projects.
- `QuantityMeasurementSolution/QuantityMeasurementSolution.slnx`
  - Slim solution containing domain, tests, API, and console app.

Use either solution based on your workflow. For full-layer development, prefer `QuantityMeasurementApp.sln`.

## Tech Stack

- .NET 8 (`net8.0`)
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- ADO.NET (`Microsoft.Data.SqlClient`)
- JWT Bearer Authentication
- MSTest

## Prerequisites

- .NET 8 SDK
- SQL Server (for database-backed modes)
- Optional: Visual Studio 2022 / VS Code + C# extension

## Configuration

### API Configuration

API settings live in `QuantityMeasurementSolution/QuantityMeasurement.Api/appsettings.json`:

- `ConnectionStrings:QuantityMeasurementDb`
- `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiryMinutes`

Replace the default JWT key with a secure secret before real use.

### Console Configuration

Console defaults are configured in `QuantityMeasurementSolution/QuantityMeasurementApp/Program.cs`:

- `useDatabaseRepository: true`
- Connection string:

```text
Server=localhost,1433;Database=QuantityMeasurementDb;User Id=sa;Password=Admin@123;TrustServerCertificate=True;
```

If you do not want SQL persistence in console mode, set `useDatabaseRepository: false`.

## Database Setup

There are two supported database paths:

1. SQL script (ADO.NET / manual setup)
	- Run `QuantityMeasurementSolution/QuantityMeasurementDb.sql`.
2. EF Core migrations (ORM / API startup)
	- API startup applies pending migrations automatically via `Database.Migrate()`.

The SQL script creates:

- `dbo.QuantityMeasurements`
- `dbo.QuantityMeasurementHistory`
- Insert-history trigger and useful indexes

## Build and Run

Run commands from repository root.

### Restore

```bash
dotnet restore QuantityMeasurementApp.sln
```

### Build

```bash
dotnet build QuantityMeasurementApp.sln
```

### Run Tests

```bash
dotnet test QuantityMeasurementSolution/QuantityMeasurement.Tests/QuantityMeasurement.Tests.csproj
```

### Run API

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurement.Api/QuantityMeasurement.Api.csproj
```

After startup:

- Swagger UI: `/swagger`
- Health endpoint: `GET /api/QuantityMeasurement/health`

### Run Console App

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurementApp/QuantityMeasurementApp.csproj
```

## API Endpoints

Base route: `/api`

Authentication (`AllowAnonymous`):

- `POST /api/Auth/signup`
- `POST /api/Auth/login`

Quantity operations (`Authorize` required except health):

- `POST /api/QuantityMeasurement/compare`
- `POST /api/QuantityMeasurement/convert`
- `POST /api/QuantityMeasurement/add`
- `POST /api/QuantityMeasurement/subtract`
- `POST /api/QuantityMeasurement/divide`
- `GET /api/QuantityMeasurement/health` (anonymous)

All API responses are wrapped with `ApiResponse<T>`.

## Repository Modes

`RepositoryLayer` contains these implementations:

- `QuantityMeasurementCacheRepository` (in-memory)
- `QuantityMeasurementDatabaseRepository` (ADO.NET SQL)
- `QuantityMeasurementOrmRepository` (EF Core)

The API is wired to ORM repository by default (`QuantityMeasurementOrmRepository`).

## Test Coverage

Current test projects/files include:

- `QuantityGenericTests.cs`
- `QuantityAdvancedOperationsTests.cs`
- `QuantityUc12Tests.cs`
- `QuantityMeasurementServiceTests.cs`

Coverage focus:

- Equality and cross-unit comparison
- Conversion accuracy
- Addition and subtraction across units
- Division rules and error scenarios
- Generic quantity behavior
- Service-level orchestration

## Detailed Use Case Progress (UC1 to UC18)

1. `UC1 - Feet Equality`
   - Compare two values in feet and return equality result.
2. `UC2 - Inches Equality and Validation`
   - Handle inch-based comparisons and basic numeric input handling.
3. `UC3 - Generic Quantity for Length`
   - Introduce generic quantity modeling for length units.
4. `UC4 - Additional Length Units`
   - Add `Yards` and `Centimeters` to length operations.
5. `UC5 - Unit Conversion`
   - Convert source quantity into an explicit target unit.
6. `UC6 - Addition Across Compatible Units`
   - Support add operation between equivalent measurement units.
7. `UC7 - Addition With Target Unit`
   - Return addition result in caller-selected output unit.
8. `UC8 - Unit-Specific Conversion Responsibility`
   - Move conversion behavior into unit/category-specific logic.
9. `UC9 - Weight Category Support`
   - Add weight domain support (`Gram`, `Kilogram`, `Pound`, `Tonne`).
10. `UC10 - Generic Quantity<TUnit> Model`
	- Standardize multi-category behavior via reusable generic quantity type.
11. `UC11 - Volume Category Support`
	- Add volume units (`Litre`, `Millilitre`, `Gallon`) and operations.
12. `UC12 - Subtraction and Division`
	- Introduce subtraction and scalar division with validation (for example divide-by-zero guard).
13. `UC13 - Centralized Arithmetic Logic`
	- Refactor add/subtract/divide flow into centralized reusable logic while preserving behavior.
14. `UC14 - Temperature Category`
	- Add temperature equality/conversion (`Celsius`, `Fahrenheit`, `Kelvin`) and block unsupported temperature arithmetic.
15. `UC15 - Service Layer Integration`
	- Expose domain operations through business service abstractions and DTO mapping.
16. `UC16 - Persistence Layer`
	- Persist measurement activity through repository layer (cache/database/ORM paths).
17. `UC17 - Web API Operations`
	- Provide HTTP endpoints for compare/convert/add/subtract/divide and health check.
18. `UC18 - Authentication and Protected API`
	- Add signup/login flow, secure password hashing, JWT issuance, and authorization on quantity endpoints.

This UC mapping reflects the current code organization across domain, business, repository, console, and API projects.

## Notes

- Keep API JWT key and database credentials environment-specific for production.
- If startup fails with DB errors, verify connection strings and SQL Server accessibility.
