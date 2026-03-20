using Howest.Lab1.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Howest.Lab1.Validations;
using Microsoft.AspNetCore.Builder;
using Howest.Lab1.Repositories;
using Howest.Lab1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<WineValidator>();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IWineRepository, WineRepository>();
builder.Services.AddScoped<IWineService, WineService>();

var app = builder.Build();

app.MapOpenApi();

app.MapGet("/wine", () => {
    return Results.Ok(new Wine()
    {
        WineID = 1,
        Name = "Muller Kogl",
        Year = 2019,
        Country = "Austria",
        Color = "White",
        Price = 12.5M,
        Grapes = "Gruner Veltliner"
    });
});

app.MapPost("/wines", async (IValidator<Wine> validator, IWineService wineService, Wine wine) => {
    var validationResult = await validator.ValidateAsync(wine);
    if(!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    wineService.AddWine(wine);
    return Results.Ok(wine);
});

app.MapGet("/wines", (IWineService wineService) => {
    return Results.Ok(wineService.GetWines());
});

app.MapGet("/wines/{id}", (IWineService wineService, int id) => {
    var wine = wineService.GetWine(id);
    if (wine == null) return Results.NotFound();
    return Results.Ok(wine);
});

app.MapDelete("/wines/{id}", (IWineService wineService, int id) => {
    var wine = wineService.GetWine(id);
    if (wine == null)
    {
        return Results.NotFound();
    }
    wineService.DeleteWine(id);
    return Results.Ok();
});

app.MapPut("/wines/{id}", async (IValidator<Wine> validator, IWineService wineService, int id, Wine wine) => {
    var validationResult = await validator.ValidateAsync(wine);
    if(!validationResult.IsValid)
    {
         return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var existingWine = wineService.GetWine(id);
    if (existingWine == null)
    {
        return Results.NotFound();
    }
    
    wine.WineID = id;
    wineService.UpdateWine(wine);
    return Results.Ok(wine);
});


app.Run("http://localhost:3000");

