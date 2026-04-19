using exercise_01_api.Data;
using exercise_01_api.Models;
using exercise_01_api.Services;
using exercise_01_api.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Register this in Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LibraryDb"));

builder.Services.AddScoped<ILibraryService, LibraryService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddErrorFilter<ValidationErrorFilter>();

var app = builder.Build();

// Ensure DB is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.MapGraphQL();
app.MapGet("/", () => "Hello World!");

app.Run();
