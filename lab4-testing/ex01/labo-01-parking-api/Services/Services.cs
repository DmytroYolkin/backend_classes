namespace labo_01_parking_api.Services;

public interface IPricingService
{
    decimal CalculatePrice(DateTime start, DateTime end);
}

public class PricingService : IPricingService
{
    private const decimal PricePerHour = 2.50m;
    private const decimal MinimumPrice = 2.00m;

    public decimal CalculatePrice(DateTime start, DateTime end)
    {
        if (end <= start)
            return 0m;

        var duration = end - start;
        var hours = (decimal)Math.Ceiling(duration.TotalHours);
        var price = hours * PricePerHour;

        return Math.Max(price, MinimumPrice);
    }
}

public interface IRegistrationService
{
    Task<RegistrationResponseDto?> StartParkingAsync(CreateRegistrationDto dto, CancellationToken ct = default);
    Task<RegistrationResponseDto?> StopParkingAsync(int registrationId, CancellationToken ct = default);
    Task<RegistrationResponseDto?> GetRegistrationAsync(int id, CancellationToken ct = default);
    Task<List<RegistrationResponseDto>> GetAllRegistrationsAsync(CancellationToken ct = default);
}

public class RegistrationService(
    IRegistrationRepository repository,
    ICarRepository carRepository,
    IPricingService pricingService,
    IEmailService emailService,
    IMapper mapper) : IRegistrationService
{
    public async Task<RegistrationResponseDto?> StartParkingAsync(CreateRegistrationDto dto, CancellationToken ct = default)
    {
        var car = await carRepository.GetByIdAsync(dto.CarId, ct);
        if (car == null)
            return null;

        var registration = new Registration
        {
            Plate = dto.Plate,
            CarId = dto.CarId,
            Start = DateTime.UtcNow,
            TotalPrice = 0m,
            IsFinished = false
        };

        await repository.AddAsync(registration, ct);
        return mapper.Map<RegistrationResponseDto>(registration);
    }

    public async Task<RegistrationResponseDto?> StopParkingAsync(int registrationId, CancellationToken ct = default)
    {
        var registration = await repository.GetByIdAsync(registrationId, ct);
        if (registration == null || registration.IsFinished)
            return null;

        registration.End = DateTime.UtcNow;
        registration.TotalPrice = pricingService.CalculatePrice(registration.Start, registration.End.Value);
        registration.IsFinished = true;

        await repository.UpdateAsync(registration, ct);

        var emailMessage = new EmailMessage(
            To: $"owner@car-{registration.Plate}.local",
            Subject: "Parking Session Completed",
            Body: $"Your parking session for plate {registration.Plate} has ended.\n" +
                  $"Duration: {registration.End.Value - registration.Start:hh\\:mm\\:ss}\n" +
                  $"Total Price: €{registration.TotalPrice:F2}"
        );

        await emailService.SendEmailAsync(emailMessage, ct);

        return mapper.Map<RegistrationResponseDto>(registration);
    }

    public async Task<RegistrationResponseDto?> GetRegistrationAsync(int id, CancellationToken ct = default)
    {
        var registration = await repository.GetByIdAsync(id, ct);
        return mapper.Map<RegistrationResponseDto>(registration);
    }

    public async Task<List<RegistrationResponseDto>> GetAllRegistrationsAsync(CancellationToken ct = default)
    {
        var registrations = await repository.GetAllAsync(ct);
        return mapper.Map<List<RegistrationResponseDto>>(registrations);
    }
}
