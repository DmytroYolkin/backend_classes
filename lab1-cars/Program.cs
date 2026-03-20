var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BrandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CarValidator>();
var app = builder.Build();

List<CarModel> cars = new List<CarModel>
{
    new CarModel()
    {
        CarModelId = 1,
        Name = "Model S",
        Brand = new Brand()
        {
            BrandId = 1,
            Name = "Tesla",
            Country = "USA",
            Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bd/Tesla_Motors.svg/2560px-Tesla_Motors.svg.png"
        }
    }
};

List<Brand> brands = new List<Brand>
{
    new Brand()
    {
        BrandId = 1,
        Name = "Tesla",
        Country = "USA",
        Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bd/Tesla_Motors.svg/2560px-Tesla_Motors.svg.png"
    },
    new Brand()
    {
        BrandId = 2,
        Name = "BMW",
        Country = "Germany",
        Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/BMW.svg/2560px-BMW.svg.png"
    }
};

app.MapGet("/", () => "Hello World!");

app.MapGet("/brands", () => brands);
app.MapGet("/brands/{id}", (int id) => {
    var brand = brands.FirstOrDefault(b => b.BrandId == id);
    if (brand == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(brand);
});

app.MapPost("/brands", async (IValidator<Brand> validator, Brand brand) => {
    var validationResult = await validator.ValidateAsync(brand);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    brand.BrandId = brands.Count() + 1;
    brands.Add(brand);
    return Results.Ok(brand);
});

app.MapGet("/brands/country/{country}", (string country) => {
    var brand = brands.FirstOrDefault(b => b.Country.ToLower() == country.ToLower());
    if (brand == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(brand);
});




app.MapGet("/cars", () => cars);
app.MapGet("/cars/brand/{brandId}", (int brandId) => {
    var car = cars.FindAll(c => c.Brand.BrandId == brandId);
    if (car == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(car);
});
app.MapGet("/cars/{id}", (int id) => {
    var car = cars.FirstOrDefault(c => c.CarModelId == id);
    if (car == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(car);
});
app.MapGet("/cars/country/{country}", (string country) => {
    var car = cars.FindAll(c => c.Brand.Country.ToLower() == country.ToLower());
    if (car == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(car);
});

app.Run();
