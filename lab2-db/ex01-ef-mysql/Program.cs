using Howest.lab2.ex01_ef_mysql.Data;
using Howest.lab2.ex01_ef_mysql.Models;
using Howest.lab2.ex01_ef_mysql.DTOs;
using Howest.lab2.ex01_ef_mysql.Mappings;
using Howest.lab2.ex01_ef_mysql.Repositories;
using Howest.lab2.ex01_ef_mysql.Services;
using Howest.lab2.ex01_ef_mysql.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PersonContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IValidator<Person>, PersonValidation>();

var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PersonContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

app.MapGet("/", () => "Hello World!");

app.MapGet("/persons", (IPersonService service) =>
{
    return Results.Ok(service.GetAllPersons());
});

app.MapGet("/persons/{id}", (int id, IPersonService service) =>
{
    var personDto = service.GetPerson(id);
    return personDto is not null ? Results.Ok(personDto) : Results.NotFound();
});

app.MapPost("/persons", async (PersonDTO personDto, IPersonService service, IValidator<Person> validator, IMapper mapper) =>
{
    var person = mapper.Map<Person>(personDto);
    var validationResult = await validator.ValidateAsync(person);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    var createdPersonDto = service.AddPerson(personDto);
    return Results.Created($"/persons/{createdPersonDto.Id}", createdPersonDto);
});

app.MapPut("/persons/{id}", async (int id, PersonDTO personDto, IPersonService service, IValidator<Person> validator, IMapper mapper) =>
{    
    var person = mapper.Map<Person>(personDto);
    var validationResult = await validator.ValidateAsync(person);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var updatedPersonDto = service.UpdatePerson(id, personDto);
    return updatedPersonDto is not null ? Results.Ok(updatedPersonDto) : Results.NotFound();
});

app.MapDelete("/persons/{id}", (int id, IPersonService service) =>
{
    var existingPerson = service.GetPerson(id); // Check first? The repository implementation checks internally for remove but passing void here.
    if (existingPerson == null) 
    {
         return Results.NotFound();
    }
    service.DeletePerson(id);
    return Results.NoContent();
});

app.Run();
