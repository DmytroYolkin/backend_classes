using Asp.Versioning;
using ex07.Data;
using ex07.DTOs;
using ex07.Endpoints;
using ex07.Exceptions;
using ex07.Repositories;
using ex07.Services;
using ex07.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext().WriteTo.Console();
});

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services and Repositories
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();

// Add Validators
builder.Services.AddValidatorsFromAssemblyContaining<ExerciseValidator>();

// Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add Caching
builder.Services.AddMemoryCache();

// Add Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var versionedGroup = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(versionSet);

app.MapExerciseEndpoints();
app.MapWorkoutEndpoints();

app.Run();
