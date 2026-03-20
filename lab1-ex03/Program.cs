
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IVaccinTypeRepository,VaccinTypeRepository>();
builder.Services.AddTransient<IVaccinationLocationRepository,VaccinationLocationRepository>();
builder.Services.AddTransient<IVaccinationRegistrationRepository,VaccinationRegistrationRepository>();
builder.Services.AddTransient<IVaccinationService,VaccinationService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<VaccinationRegistrationValidator>();

builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));

app.MapGet("/", () => "Hello World!");

app.MapGet("/locations",(IVaccinationService vaccinationService) =>
{
    return Results.Ok(vaccinationService.GetLocations());
});

app.MapPost("/registrations", (IValidator<VaccinRegistration> validator, IVaccinationService vaccinationService, VaccinRegistration registration) =>
{
    var validationResult = validator.Validate(registration);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    var result = vaccinationService.AddRegistration(registration);
    return Results.Ok(result);
});

app.MapGet("/registrations", (IMapper mapper, IVaccinationService vaccinationService) =>
{
    var mapped = mapper.Map<List<VaccinRegistrationDTO>>(vaccinationService.GetRegistrations());
    return Results.Ok(mapped);
});

app.MapGet("/vaccins",(IVaccinationService vaccinationService) =>
{
    return Results.Ok(vaccinationService.GetVaccins());
});

app.Run();
