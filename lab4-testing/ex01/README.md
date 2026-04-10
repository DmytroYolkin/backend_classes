# Parking API

A modern .NET 10 ASP.NET Core API built with clean architecture principles for managing car parking sessions.

## Features

- **Car Management**
  - Create new cars
  - View all cars
  - Validate license plates

- **Parking Sessions**
  - Start parking sessions
  - Stop parking sessions with automatic price calculation
  - View all registrations

- **Pricing Service**
  - Hourly rate: €2.50/hour
  - Minimum charge: €2.00
  - Charges by full hours (rounded up)

## Technology Stack

- **.NET 10** with C# 14
- **ASP.NET Core Minimal APIs** (no controllers)
- **Entity Framework Core** with PostgreSQL
- **FluentValidation** for input validation
- **AutoMapper** for DTO mapping
- **xUnit** for testing
- **FluentAssertions** for assertion fluency

## Prerequisites

- .NET 10 SDK
- Docker (for PostgreSQL)
- Docker Compose

## Setup

### 1. Start PostgreSQL
```bash
docker-compose up -d
```

### 2. Restore packages
```bash
dotnet restore
```

### 3. Build the solution
```bash
dotnet build
```

### 4. Run the API
```bash
cd labo-01-parking-api
dotnet watch run
```

## Running Tests

```bash
dotnet test
```

Test coverage includes:
- **License Plate Validation** (15 tests)
  - Valid and invalid plate formats
  - Case insensitivity
  - Edge cases

- **Car Creation Validation** (8 tests)
  - Required fields validation
  - License plate validation integration
  - Field length constraints

- **Pricing Calculation** (17 tests)
  - Various parking durations
  - Minimum charge application
  - Ceiling hour calculation

## API Endpoints

### Cars

#### Create a new car
```http
POST /api/cars
Content-Type: application/json

{
  "brand": "Toyota",
  "model": "Corolla",
  "plate": "ABC-123",
  "color": "Blue"
}
```

**Response (201 Created)**
```json
{
  "id": 1,
  "brand": "Toyota",
  "model": "Corolla",
  "plate": "ABC-123",
  "color": "Blue"
}
```

#### Get all cars
```http
GET /api/cars
```

**Response (200 OK)**
```json
[
  {
    "id": 1,
    "brand": "Toyota",
    "model": "Corolla",
    "plate": "ABC-123",
    "color": "Blue"
  }
]
```

### Registrations (Parking Sessions)

#### Start parking
```http
POST /api/registrations/start
Content-Type: application/json

{
  "plate": "ABC-123",
  "carId": 1
}
```

**Response (201 Created)**
```json
{
  "id": 1,
  "plate": "ABC-123",
  "end": null,
  "totalPrice": 0.00,
  "isFinished": false
}
```

#### Stop parking
```http
PUT /api/registrations/{id}/stop
```

**Response (200 OK)**
```json
{
  "id": 1,
  "plate": "ABC-123",
  "end": "2026-04-09T14:35:22.123Z",
  "totalPrice": 5.00,
  "isFinished": true
}
```

#### Get all registrations
```http
GET /api/registrations
```

## Database Connection

PostgreSQL connection string (localhost):
```
Host=localhost;Port=5410;Database=backend_week04_labo01;Username=postgres;Password=postgres
```

## Project Structure

```
labo-01-parking-api/
├── Context/
│   ├── ParkingDbContext.cs
│   └── Usings.cs
├── DTO/
│   └── DtoModels.cs
├── Http/
│   ├── Endpoints.cs
│   └── parking-api.http
├── Models/
│   ├── Car.cs
│   └── Registration.cs
├── Repositories/
│   └── Repositories.cs
├── Services/
│   └── Services.cs
├── Validators/
│   └── Validators.cs
├── Mappings/
│   └── MappingProfile.cs
├── Program.cs
└── appsettings.json

labo-01-parking-test/
├── UnitTest1.cs
├── Usings.cs
└── labo-01-parking-test.csproj
```

## Testing Scenarios

### License Plate Validation
- ✅ Valid formats: ABC-123, XX-1234, TEST-PLATE
- ❌ Invalid formats: Empty, too short, special characters, Unicode

### Price Calculation
- ✅ Charges per full hour rounded up
- ✅ Applies €2.00 minimum
- ✅ Calculates correctly for various durations

## Error Handling

All endpoints return proper HTTP status codes and problem details:
- **201 Created** - Successfully created resource
- **200 OK** - Successful retrieval or update
- **400 Bad Request** - Invalid input with validation errors
- **404 Not Found** - Resource not found

## Notes

- All datetime values are stored in UTC
- License plate validation is case-insensitive
- Parking sessions are immutable once stopped
- Database uses decimal(8,2) for price precision
