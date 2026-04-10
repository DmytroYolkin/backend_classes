using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using labo_01_parking_api.Context;
using Testcontainers.PostgreSql;

namespace labo_01_parking_test;

public class ParkingWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    public ParkingWebApplicationFactory()
    {
        DotNetEnv.Env.Load();
        
        _dbContainer = new PostgreSqlBuilder()
            .WithImage(Environment.GetEnvironmentVariable("POSTGRES_IMAGE") ?? "postgres:latest")
            .WithDatabase(Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "parking_db")
            .WithUsername(Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres")
            .WithPassword(Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real DbContext registration
            services.RemoveAll<DbContextOptions<ParkingDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<ParkingDbContext>>();
            
            // Add PostgreSQL Database with TestContainers connection string
            services.AddDbContext<ParkingDbContext>(options => 
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            // Initialize the database
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });

        base.ConfigureWebHost(builder);
    }
}
