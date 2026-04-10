using Howest.lab2.ex04_sensor_monitoring.Data;
using Howest.lab2.ex04_sensor_monitoring.DTOs;
using Howest.lab2.ex04_sensor_monitoring.Services;
using Howest.lab2.ex04_sensor_monitoring.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Database
builder.Services.AddSingleton<ISensorRepository, SensorRepository>();
builder.Services.AddSingleton<IReadingRepository, ReadingRepository>();

// Services
builder.Services.AddScoped<ISensorService, SensorService>();

// SMS Client
builder.Services.AddHttpClient<ISmsService, SmsService>(client =>
{
    var baseUrl = builder.Configuration["SmsApiSettings:BaseUrl"] ?? "https://fake-sms-api-hygzh5f5fnc2d7ay.canadacentral-01.azurewebsites.net/";
    client.BaseAddress = new Uri(baseUrl);
});

// Mapping & Validation
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<SensorCreateValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------------------------------------------------------
// Endpoints
// ---------------------------------------------------------

app.MapGet("/sensors", async (ISensorService service) => 
    Results.Ok(await service.GetSensorsAsync()));

app.MapGet("/sensors/{id}", async (string id, ISensorService service) =>
{
    var sensor = await service.GetSensorAsync(id);
    return sensor is not null ? Results.Ok(sensor) : Results.NotFound(new { Message = $"Sensor with ID {id} not found." });
});

app.MapPost("/sensors", async (SensorCreateDto dto, ISensorService service, IValidator<SensorCreateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var sensor = await service.CreateSensorAsync(dto);
    return Results.Created($"/sensors/{sensor.Id}", sensor);
});

app.MapDelete("/sensors/{id}", async (string id, ISensorService service) =>
{
    var deleted = await service.DeleteSensorAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound(new { Message = $"Sensor with ID {id} not found." });
});

app.MapGet("/sensors/{id}/readings", async (string id, ISensorService service) =>
{
    var sensor = await service.GetSensorAsync(id);
    if (sensor == null) return Results.NotFound(new { Message = $"Sensor with ID {id} not found." });
    
    var readings = await service.GetReadingsAsync(id);
    return Results.Ok(readings);
});

app.MapPost("/sensors/{id}/readings", async (string id, ReadingCreateDto dto, ISensorService service, IValidator<ReadingCreateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var reading = await service.AddReadingAsync(id, dto);
    return reading is not null ? Results.Created($"/sensors/{id}/readings/{reading.Id}", reading) : Results.NotFound(new { Message = $"Sensor with ID {id} not found." });
});

app.Run();
