# Quantity Measurement Application

Build, compare, convert, and calculate quantities across multiple measurement categories using a clean .NET layered architecture.

This project started from a simple UC1 requirement (Feet equality) and evolved into a reusable, test-driven solution with:
- Generic quantity handling (`Quantity<TUnit>`)
- Multiple categories (Length, Weight, Volume, Temperature)
- Console app + Web API support
- Strong validation and unit-test coverage (MSTest)

## Why This Project Is Useful

- Easy to extend with new units and categories
- Clear separation of concerns (Core, Service, Repository, API)
- Backward-compatible evolution from UC1 to UC15
- Includes operation history support via repository abstraction

## Current Solution Layout

```text
QuantityMeasurementSolution/
|- QuantityMeasurement.Api                  ASP.NET Core Web API
|- QuantityMeasurement.BusinessLayer        Service layer and use-case orchestration
|- QuantityMeasurement.Core                 Generic quantity model and unit conversions
|- QuantityMeasurement.Domain               Domain contracts and domain-centric components
|- QuantityMeasurement.Entities             Persistence/entity models
|- QuantityMeasurement.Infrastructure       Infrastructure implementations
|- QuantityMeasurement.Models               DTOs and transport models
|- QuantityMeasurement.Repository           Repository implementation (cache/history)
|- QuantityMeasurement.Repository.Abstractions
|- QuantityMeasurement.Tests                MSTest test suite
`- QuantityMeasurementApp                   Console application
```

## Supported Measurement Categories

- Length: Feet, Inches, Yards, Centimeters
- Weight: Gram, Kilogram, Tonne
- Volume: Litre, Millilitre, Gallon
- Temperature: Celsius, Fahrenheit, Kelvin

Note:
Temperature supports equality and conversion, but arithmetic on absolute temperatures is intentionally restricted.

## Quick Start

Run from the repository root.

### Restore

```bash
dotnet restore QuantityMeasurementSolution/QuantityMeasurementSolution.slnx
```

### Run Console App

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurementApp
```

### Run API

```bash
dotnet run --project QuantityMeasurementSolution/QuantityMeasurement.Api
```

Then open Swagger (URL shown in terminal, usually `/swagger`) to test endpoints.

### Run Tests

```bash
dotnet test QuantityMeasurementSolution/QuantityMeasurement.Tests
```

## UC Progress

- [x] UC1: Feet equality comparison
- [x] UC2: Inches equality + numeric validation
- [x] UC3: Generic quantity model for length (DRY)
- [x] UC4: Added Yards and Centimeters
- [x] UC5: Unit-to-unit conversion
- [x] UC6: Addition across length units
- [x] UC7: Addition with explicit target unit
- [x] UC8: Refactored conversion responsibility to unit enums
- [x] UC9: Added weight support
- [x] UC10: Introduced `Quantity<TUnit>` + measurable abstraction
- [x] UC11: Added volume support
- [x] UC12: Added subtraction and division operations
- [x] UC13: Centralized arithmetic logic (DRY refactor)
- [x] UC14: Added temperature with selective arithmetic restrictions
- [x] UC15: Added Web API layer with reusable N-tier architecture

## Technical Highlights

- Immutable value-object style quantity modeling
- Value-based equality with precision handling for floating-point comparisons
- Consistent exception strategy through domain-specific exception types
- Service and repository abstractions for testability and extensibility
- Regression-safe evolution validated through MSTest suites

## Testing Focus Areas

- Equality and cross-unit equivalence
- Conversion accuracy
- Addition and subtraction behavior (including target unit output)
- Division behavior and divide-by-zero protection
- Temperature conversion and unsupported arithmetic safeguards
- Service-layer behavior used by API endpoints
