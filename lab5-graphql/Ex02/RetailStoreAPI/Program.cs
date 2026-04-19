using FluentValidation;
using HotChocolate.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RetailStoreAPI.Data;
using RetailStoreAPI.Endpoints;
using RetailStoreAPI.GraphQL;
using RetailStoreAPI.Validators;
using RetailStoreAPI.Repositories;
using RetailStoreAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<AddProductValidator>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStoreService, StoreService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();



var app = builder.Build();

app.MapStoreEndpoints();
app.MapGraphQL();

app.Run();
