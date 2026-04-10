# Email Notification Implementation

## Overview
Updated the parking API to send email notifications when a parking session ends (registration is stopped).

## Changes Made

### 1. New Email Service
**File**: `labo-01-parking-api/Services/EmailService.cs`
- Created `EmailMessage` record for email data
- Created `IEmailService` interface
- Created `EmailService` implementation

```csharp
public record EmailMessage(string To, string Subject, string Body);

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken ct = default);
}
```

### 2. Updated RegistrationService
**File**: `labo-01-parking-api/Services/Services.cs`
- Added `IEmailService` to primary constructor
- Updated `StopParkingAsync` to send email when parking stops
- Email includes:
  - Owner email (based on license plate)
  - Subject: "Parking Session Completed"
  - Body with plate, duration, and total price

### 3. Dependency Injection Registration
**File**: `labo-01-parking-api/Program.cs`
- Added: `builder.Services.AddScoped<IEmailService, EmailService>();`

### 4. Comprehensive Tests
**File**: `labo-01-parking-test/UnitTest1.cs`

#### Test Strategy
- **Fakes for Data Access**: `FakeCarRepository` and `FakeRegistrationRepository` (in-memory implementations)
- **Mocks for External Dependencies**: `Mock<IEmailService>` for verifying email service interactions

#### New Tests (4 total)
1. **StopParking_WithValidRegistration_SendsEmailWithCorrectData**
   - Verifies email is sent when parking stops
   - Confirms email contains correct recipient, subject, and plate info
   - Uses Moq to verify `SendEmailAsync` was called exactly once with correct parameters

2. **StopParking_WithValidRegistration_CalculatesPriceCorrectly**
   - Verifies price calculation occurs before sending email
   - Ensures pricing logic remains correct

3. **StopParking_WithAlreadyFinishedRegistration_ReturnsNull**
   - Verifies email is NOT sent for already-finished sessions
   - Uses Moq to verify `SendEmailAsync` was never called

4. **StopParking_WithNonExistentRegistration_ReturnsNull**
   - Verifies email is NOT sent for non-existent registrations
   - Uses Moq to verify `SendEmailAsync` was never called

### 5. Dependencies Added
- **Moq** (v4.20.72) for mocking external services

## Test Results

```
Total Tests: 44
✅ Passed: 44
❌ Failed: 0
⏭️ Skipped: 0
```

### Test Breakdown
- **LicensePlateValidatorTests**: 15 tests
- **CreateCarValidatorTests**: 8 tests
- **PricingServiceTests**: 17 tests
- **RegistrationServiceTests**: 4 tests (NEW - Email functionality)

## Testing Approach - Fakes vs Mocks

### Fakes (In-Memory Repositories)
```csharp
internal class FakeCarRepository : ICarRepository
{
    private readonly List<Car> _cars = [];
    // In-memory implementations of all repository methods
}

internal class FakeRegistrationRepository : IRegistrationRepository
{
    private readonly List<Registration> _registrations = [];
    // In-memory implementations of all repository methods
}
```
**When to use**: Testing data access, repository logic, state management

### Mocks (External Services)
```csharp
var mockEmailService = new Mock<IEmailService>();

// Verify interaction
mockEmailService.Verify(
    x => x.SendEmailAsync(
        It.Is<EmailMessage>(msg => 
            msg.To == "owner@car-ABC-123.local" &&
            msg.Subject == "Parking Session Completed"),
        It.IsAny<CancellationToken>()),
    Times.Once);
```
**When to use**: Testing external service dependencies, verifying method calls/interactions

## Email Format Example

When a parking session ends, an email is sent:
```
To: owner@car-ABC-123.local
Subject: Parking Session Completed
Body:
  Your parking session for plate ABC-123 has ended.
  Duration: 02:15:30
  Total Price: €5.75
```


