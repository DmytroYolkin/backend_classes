var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ILicensePlateValidator, LicensePlateValidator>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCarValidator>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapCarEndpoints();
app.MapRegistrationEndpoints();

app.Run();
