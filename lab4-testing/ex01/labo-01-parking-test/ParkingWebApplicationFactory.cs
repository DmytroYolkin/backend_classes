using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using labo_01_parking_api.Context;

namespace labo_01_parking_test;

public class ParkingWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real DbContext registration
            services.RemoveAll<DbContextOptions<ParkingDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<ParkingDbContext>>();
            
            // Add InMemory Database for testing
            services.AddDbContext<ParkingDbContext>(options => 
                options.UseInMemoryDatabase("InMemoryDbForTesting_Parking"));

            // Initialize the database
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });

        base.ConfigureWebHost(builder);
    }
}
