var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.Configure<DatabaseSettings>(configuration.GetSection("MongoConnection"));
builder.Services.AddTransient<IMongoContext, MongoContext>();

// Register Repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

// Register Services
builder.Services.AddScoped<ICarService, CarService>();

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<AutoMapperProfile>();
});

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CarValidator>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Setup Route
app.MapGet("/setup", async (ICarService carService) =>
{
    await carService.SetupDummyData();
    return Results.Ok("Dummy data inserted.");
});

// API Routes
app.MapGet("/cars", async (ICarService carService) =>
{
    return Results.Ok(await carService.GetCars());
});

app.MapGet("/brands", async (ICarService carService) =>
{
    return Results.Ok(await carService.GetBrands());
});

app.MapPost("/cars", async (ICarService carService, CarDTO car) =>
{
    try 
    {
        var result = await carService.AddCar(car);
        return Results.Created($"/cars", result);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(ex.Errors);
    }
});

app.MapPost("/brands", async (ICarService carService, BrandDTO brand) =>
{
    try 
    {
        var result = await carService.AddBrand(brand);
        return Results.Created($"/brands", result);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(ex.Errors);
    }
});

app.Run();
