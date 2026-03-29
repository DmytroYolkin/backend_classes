using Howest.ex05.Data;
using Howest.ex05.Repositories;
using Howest.ex05.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Increase Max Request Limits for Kestrel (to allow huge CSV files up to ~200MB)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 250_000_000;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = 250_000_000; 
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddDbContext<SteamContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SteamDb")));

builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<ImportService>();

var app = builder.Build();

app.MapPost("/api/import", async (HttpRequest request, ImportService importService) =>
{
    if (!request.HasFormContentType || request.Form.Files.Count < 3)
    {
        return Results.BadRequest("Please upload all 3 CSV files (steam.csv, steam_description_data.csv, steam_requirements_data.csv) using multipart/form-data.");
    }

    try
    {
        var timeTaken = await importService.ImportFromFilesAsync(request.Form.Files);
        return Results.Ok(new { Message = "Imported successfully.", TimeElapsed = timeTaken.TotalSeconds + "s" });
    }
    catch(ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/games/search", async (GameRepository repo, string q) =>
{
    return Results.Ok(await repo.SearchFullTextAsync(q));
});

app.MapGet("/api/games/tag/{tagName}", async (GameRepository repo, string tagName) =>
{
    return Results.Ok(await repo.SearchTagsAsync(tagName));
});

app.MapGet("/api/games/requirements", async (GameRepository repo, string query) =>
{
    return Results.Ok(await repo.SearchRequirementsAsync(query));
});

// Auto Apply DB Schema creation for testing purposes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SteamContext>();
    db.Database.EnsureCreated(); 
}

app.Run();
