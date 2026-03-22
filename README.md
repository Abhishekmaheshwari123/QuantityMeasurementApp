# Quantity Measurement Application

Build, compare, convert, and calculate quantities across multiple measurement categories using a layered .NET solution.

The codebase started with the UC1 feet-equality requirement and has grown into a reusable quantity-measurement application with:

- Generic quantity modeling through `Quantity<TUnit>`
- Multiple categories: length, weight, volume, and temperature
- Console and ASP.NET Core API entry points
- MSTest coverage for domain and service behavior
- Optional persistence through an in-memory repository or SQL Server via ADO.NET

## What The Application Supports

- Compare quantities across equivalent units
- Convert quantities into a target unit
- Add and subtract compatible quantities
- Divide compatible quantities to produce ratios
- Restrict unsupported arithmetic on absolute temperatures
- Persist measurement history through the repository layer

## Supported Units

- Length: Feet, Inches, Yards, Centimeters
- Weight: Gram, Kilogram, Pound, Tonne
- Volume: Litre, Millilitre, Gallon
- Temperature: Celsius, Fahrenheit, Kelvin

Temperature supports comparison and conversion. Arithmetic on temperatures is intentionally rejected by the domain rules.

## Repository Layout

The repository currently contains both the active solution projects and supporting layer projects referenced by the app and API.

```text
QuantityMeasurementSolution/
|- QuantityMeasurementSolution.slnx
|- QuantityMeasurement.Domain/            Domain units, value objects, exceptions, services
|- QuantityMeasurement.Infrastructure/    Infrastructure project referenced by the API
|- QuantityMeasurement.Tests/             MSTest test suite
|- QuantityMeasurementApp/                Console application
|- QuantityMeasurement.Api/               ASP.NET Core Web API
|- BusinessLayer/                         Application/service layer used by app and API
|- ModelLayer/                            DTOs, entities, and models
|- RepositoryLayer/                       In-memory and SQL Server repository implementations
`- QuantityMeasurementDb.sql              SQL Server schema setup script
```

## Runtime Behavior

- Console app: configured to use the SQL Server repository by default
- Web API: currently configured to use the in-memory cache repository
- Repository abstraction: supports saving measurements, querying history, counting records, clearing records, and releasing resources

This means the console app requires a working SQL Server database unless you change its startup configuration.

## Prerequisites

- .NET 10 SDK
- SQL Server instance if you want to run the console app with the default configuration

The console app currently uses this local SQL Server target:

```text
Server=localhost,1433;Database=QuantityMeasurementDb;User Id=sa;Password=Admin@123;TrustServerCertificate=True;
```

If your SQL Server runs elsewhere, update the connection string in `QuantityMeasurementSolution/QuantityMeasurementApp/Program.cs` before running the console application.

If you do not want to use SQL Server, set `useDatabaseRepository: false` in the `ApplicationConfig` created in `QuantityMeasurementSolution/QuantityMeasurementApp/Program.cs`.

## Quick Start

Run commands from the repository root.

### Restore

```bash
dotnet restore QuantityMeasurementSolution/QuantityMeasurementSolution.slnx
```

### Run Tests

```bash
dotnet test QuantityMeasurementSolution/QuantityMeasurement.Tests/QuantityMeasurement.Tests.csproj
```

### Run The API

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurement.Api/QuantityMeasurement.Api.csproj
```

Swagger is enabled by default. After the API starts, open the reported `/swagger` URL.

### Run The Console App

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurementApp/QuantityMeasurementApp.csproj
```

By default, the console app starts with the database repository enabled.

## Database Setup

Before running the console app with the default configuration, create the database objects from the provided SQL script:

```text
QuantityMeasurementSolution/QuantityMeasurementDb.sql
```

The script creates:

- `dbo.QuantityMeasurements` for stored operation results
- `dbo.QuantityMeasurementHistory` for audit-style insert tracking
- supporting indexes for operation, measurement type, and creation time queries

## API Surface

The Web API exposes these endpoints under `/api/QuantityMeasurement`:

- `POST /compare`
- `POST /convert`
- `POST /add`
- `POST /subtract`
- `POST /divide`

Requests use transport contracts in `QuantityMeasurement.Api/Contracts`, and responses are wrapped in a standard `ApiResponse<T>` payload.

## Use Case Coverage

- [x] UC1: Feet equality comparison
- [x] UC2: Inches equality and numeric validation
- [x] UC3: Generic quantity model for length
- [x] UC4: Added yards and centimeters
- [x] UC5: Unit-to-unit conversion
- [x] UC6: Addition across length units
- [x] UC7: Addition with explicit target unit
- [x] UC8: Conversion responsibility moved into unit-specific logic
- [x] UC9: Added weight support
- [x] UC10: Introduced `Quantity<TUnit>`-based generic handling
- [x] UC11: Added volume support
- [x] UC12: Added subtraction and division
- [x] UC13: Centralized arithmetic logic
- [x] UC14: Added temperature with selective arithmetic restrictions
- [x] UC15: Added Web API support
- [x] UC16: Added SQL Server persistence through ADO.NET

## Testing Focus

The test suite covers:

- Equality and cross-unit equivalence
- Conversion accuracy across categories
- Addition, subtraction, and explicit target-unit behavior
- Division and divide-by-zero behavior
- Temperature conversion and unsupported operations
- Service-layer orchestration behavior
