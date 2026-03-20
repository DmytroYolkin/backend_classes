using Howest.Lab1.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Howest.Lab1.Validations;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<WineValidator>();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

List<Wine> wines = new List<Wine>
{
    new Wine()
    {
        WineID = 1,
        Name = "Muller Kogl",
        Year = 2019,
        Country = "Austria",
        Color = "White",
        Price = 12.5M,
        Grapes = "Gruner Veltliner"
    }
};

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

app.MapPost("/wines", async (IValidator<Wine> validator, Wine wine) => {
    var validationResult = await validator.ValidateAsync(wine);
    if(!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    wine.WineID = wines.Count() + 1;
    wines.Add(wine);
    return Results.Ok(wine);
});

app.MapGet("/wines", () => {
    return Results.Ok(wines);
});

app.MapDelete("/wines/{id}", (int id) => {
    var wine = wines.FirstOrDefault(w => w.WineID == id);
    if (wine == null)
    {
        return Results.NotFound();
    }
    wines.Remove(wine);
    return Results.Ok();
});

app.MapPut("/wines", (int id, Wine wine) => {
    var existingWine = wines.FirstOrDefault(w => w.WineID == id);
    if (existingWine == null)
    {
        return Results.NotFound();
    }
    existingWine.Name = wine.Name;
    existingWine.Year = wine.Year;
    // existingWine.Country = wine.Country;
    // existingWine.Color = wine.Color;
    // existingWine.Price = wine.Price;
    // existingWine.Grapes = wine.Grapes;
    return Results.Ok(existingWine);
});


app.Run("http://localhost:3000");

