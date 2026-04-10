namespace labo_01_parking_api.Http;

public static class CarEndpoints
{
    public static void MapCarEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cars")
            .WithTags("Cars");

        group.MapPost("/", CreateCar)
            .WithName("CreateCar");

        group.MapGet("/", GetAllCars)
            .WithName("GetAllCars");
    }

    private static async Task<IResult> CreateCar(
        CreateCarDto dto,
        ICarService service,
        IValidator<CreateCarDto> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(dto, ct);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var result = await service.CreateCarAsync(dto, ct);
        return result != null ? Results.Created($"/api/cars/{result.Id}", result) : Results.BadRequest();
    }

    private static async Task<IResult> GetAllCars(
        ICarService service,
        CancellationToken ct)
    {
        var cars = await service.GetAllCarsAsync(ct);
        return Results.Ok(cars);
    }
}

public static class RegistrationEndpoints
{
    public static void MapRegistrationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/registrations")
            .WithTags("Registrations");

        group.MapPost("/start", StartParking)
            .WithName("StartParking");

        group.MapPut("/{id}/stop", StopParking)
            .WithName("StopParking");

        group.MapGet("/", GetAllRegistrations)
            .WithName("GetAllRegistrations");
    }

    private static async Task<IResult> StartParking(
        CreateRegistrationDto dto,
        IRegistrationService service,
        IValidator<CreateRegistrationDto> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(dto, ct);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var result = await service.StartParkingAsync(dto, ct);
        return result != null ? Results.Created($"/api/registrations/{result.Id}", result) : Results.NotFound("Car not found");
    }

    private static async Task<IResult> StopParking(
        int id,
        IRegistrationService service,
        CancellationToken ct)
    {
        var result = await service.StopParkingAsync(id, ct);
        return result != null ? Results.Ok(result) : Results.NotFound("Registration not found or already finished");
    }

    private static async Task<IResult> GetAllRegistrations(
        IRegistrationService service,
        CancellationToken ct)
    {
        var registrations = await service.GetAllRegistrationsAsync(ct);
        return Results.Ok(registrations);
    }
}
