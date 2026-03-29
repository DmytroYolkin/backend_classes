using Howest.ex05.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.ex05.Data;

public class SteamContext : DbContext
{
    public SteamContext(DbContextOptions<SteamContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .OwnsOne(g => g.Requirements, r =>
            {
                r.ToJson();
            });

        modelBuilder.Entity<Game>()
            .HasGeneratedTsVectorColumn(
                g => g.SearchVector,
                "english",
                g => new { g.Name, g.DetailedDescription, g.AboutTheGame, g.Developer, g.Publisher }
            )
            .HasIndex(g => g.SearchVector)
            .HasMethod("GIN");
    }
}