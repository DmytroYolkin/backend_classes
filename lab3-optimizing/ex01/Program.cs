using ex03_ef_postgresql.Data;
using ex03_ef_postgresql.Models;
using ex03_ef_postgresql.Validators;
using ex03_ef_postgresql.DTOs;
using ex03_ef_postgresql.Repositories;
using ex03_ef_postgresql.Services;
using ex03_ef_postgresql.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Serilog;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add API versioning
builder.Services.AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add DbContext
builder.Services.AddDbContext<TravelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Exception Handler
builder.Services.AddProblemDetails();

// Add validators
builder.Services.AddScoped<IValidator<Traveler>, TravelerValidator>();
builder.Services.AddScoped<IValidator<Destination>, DestinationValidator>();

// Add repositories
builder.Services.AddScoped<ITravelerRepository, TravelerRepository>();
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IGuideRepository, GuideRepository>();

// Add services
builder.Services.AddScoped<ITravelerService, TravelerService>(); // Note: implementation uses IValidator
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IFileService, FileService>();

// Configure JSON options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception is ResourceNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
        else if (exception is PassportAlreadyExistsException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
    });
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

var travelersGroup = app.NewVersionedApi("Travelers").MapGroup("/travelers").HasApiVersion(2.0);

// 1. Show all travelers
travelersGroup.MapGet("/", async (ITravelerService service, IMemoryCache cache, ILogger<Program> logger) =>
{
    const string cacheKey = "all_travelers";
    if (!cache.TryGetValue(cacheKey, out List<TravelerDto>? travelers))
    {
        travelers = await service.GetAllTravelersAsync();
        cache.Set(cacheKey, travelers, TimeSpan.FromMinutes(5));
    }
    
    logger.LogDebug($"Get all travelers {JsonSerializer.Serialize(travelers)}");
    return Results.Ok(travelers);
})
.WithName("GetTravelers");

// 2. Show a traveler
travelersGroup.MapGet("/{id}", async (int id, ITravelerService service) =>
{
    var traveler = await service.GetTravelerByIdAsync(id);
    return traveler is not null ? Results.Ok(traveler) : Results.NotFound();
})
.WithName("GetTravelerById");

// 6. Add new traveler
travelersGroup.MapPost("/", async (TravelerInputDto input, ITravelerService service) =>
{
    try
    {
        var traveler = await service.AddTravelerAsync(input);
        return Results.Created($"/travelers/{traveler.TravelerId}", traveler);
    }
    catch (ArgumentException ex) // Passport exists
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (ValidationException ex)
    {
        return Results.ValidationProblem(ex.Errors.GroupBy(x => x.PropertyName).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()));
    }
})
.WithName("CreateTraveler");

// 8. Add a traveler to a destination
travelersGroup.MapPost("/{travelerId}/destinations/{destinationId}", async (int travelerId, int destinationId, ITravelerService service) =>
{
    var result = await service.AddTravelerToDestinationAsync(travelerId, destinationId);
    
    if (result == null) 
    {
        // Could be refined to know which one wasn't found, but for now generic 404 is okay as per minimal requirements
        return Results.NotFound("Traveler or Destination not found");
    }

    return Results.Ok(result);
})
.WithName("AddTravelerToDestination");

var destinationsApi = app.NewVersionedApi("Destinations");
var destinationsV2Group = destinationsApi.MapGroup("/destinations").HasApiVersion(2.0);

// Show all destinations V2
destinationsV2Group.MapGet("/", async (IDestinationService service, IMemoryCache cache, ILogger<Program> logger) =>
{
    const string cacheKey = "all_destinations_v2";
    if (!cache.TryGetValue(cacheKey, out List<DestinationV2Dto>? destinations))
    {
        destinations = await service.GetAllDestinationsV2Async();
        cache.Set(cacheKey, destinations, TimeSpan.FromMinutes(5));
    }
    
    logger.LogDebug($"Get all destinations {JsonSerializer.Serialize(destinations)}");
    return Results.Ok(destinations);
})
.WithName("GetDestinationsV2");

// 7. Add new destination V2
destinationsV2Group.MapPost("/", async (DestinationDto input, IDestinationService service) =>
{
    try 
    {
        var destination = await service.AddDestinationAsync(input);
        return Results.Created($"/destinations/{destination.DestinationId}", destination);
    }
    catch (ValidationException ex)
    {
        return Results.ValidationProblem(ex.Errors.GroupBy(x => x.PropertyName).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()));
    }
})
.WithName("CreateDestinationV2");

var guidesGroup = app.NewVersionedApi("Guides").MapGroup("/guides").HasApiVersion(2.0);

// 4. Show all guides
guidesGroup.MapGet("/", async (string? include, IGuideService service) =>
{
    bool includeTours = include?.ToLower() == "tours";
    var guides = await service.GetAllGuidesAsync(includeTours);
    return Results.Ok(guides);
})
.WithName("GetGuides");

var uploadsApi = app.NewVersionedApi("Uploads");
var uploadsGroup = uploadsApi.MapGroup("/upload").HasApiVersion(2.0);

// 9. Upload CSV file
uploadsGroup.MapPost("/", async (IFormFile file, IFileService fileService) =>
{
    try
    {
        var filePath = await fileService.SaveFileAsync(file);
        return Results.Ok(new { message = "File uploaded successfully", path = filePath });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("UploadFile")
.Accepts<IFormFile>("multipart/form-data")
.DisableAntiforgery();

// 10. Write Database Data to CSV
uploadsGroup.MapGet("/export", async (IFileService fileService) =>
{
    var (content, fileName) = await fileService.ExportDatabaseToCsvAsync();
    return Results.File(content, "text/csv", fileName);
})
.WithName("ExportToCsv");

// 5. Get Guide based on Id
guidesGroup.MapGet("/{id}", async (int id, string? include, IGuideService service) =>
{
    bool includeTours = include?.ToLower() == "tours";
    var guide = await service.GetGuideByIdAsync(id, includeTours);
    return guide is not null ? Results.Ok(guide) : Results.NotFound();
})
.WithName("GetGuideById");

app.Run();

