using Microsoft.EntityFrameworkCore;
using ex03_ef_postgresql.Models; // assuming namespace

namespace ex03_ef_postgresql.Data;

public class TravelDbContext : DbContext
{
    public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Guide> Guides => Set<Guide>();
    public DbSet<Passport> Passports => Set<Passport>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Traveler> Travelers => Set<Traveler>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Destination: Name required, max 200
        modelBuilder.Entity<Destination>()
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Guide: Name required, max 200
        modelBuilder.Entity<Guide>()
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Tour: Title required, max 200
        modelBuilder.Entity<Tour>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Passport: PassportNumber max 10, required
        modelBuilder.Entity<Passport>()
            .Property(p => p.PassportNumber)
            .IsRequired()
            .HasMaxLength(10);

        // Traveler: Fullname max 100
        modelBuilder.Entity<Traveler>()
            .Property(t => t.FullName)
            .HasMaxLength(100);

        // Relationships

        // Traveler 1-1 Passport
        modelBuilder.Entity<Traveler>()
            .HasOne(t => t.Passport)
            .WithOne(p => p.Traveler)
            .HasForeignKey<Passport>(p => p.TravelerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Guide 1-N Tour
        modelBuilder.Entity<Guide>()
            .HasMany(g => g.Tours)
            .WithOne(t => t.Guide)
            .HasForeignKey(t => t.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        // Destination N-N Traveler
        // In EF Core 5+, Many-to-Many is automatically handled if there's no extra payload on the join table.
        // We can just rely on the navigation properties.
        modelBuilder.Entity<Destination>()
            .HasMany(d => d.Travelers)
            .WithMany(t => t.Destinations)
            .UsingEntity(j => j.ToTable("TravelerDestinations"));

        // Seed data
        modelBuilder.Entity<Destination>().HasData(
            new Destination { Id = 1, Name = "New York City, New York" },
            new Destination { Id = 2, Name = "Paris, France" },
            new Destination { Id = 3, Name = "Tokyo, Japan" }
        );

        modelBuilder.Entity<Guide>().HasData(
            new Guide { Id = 1, Name = "Robby Rix" },
            new Guide { Id = 2, Name = "Sally Guide" }
        );

        modelBuilder.Entity<Tour>().HasData(
             new { Id = 1, Title = "City Walk", GuideId = 1 },
             new { Id = 2, Title = "Museum Tour", GuideId = 1 },
             new { Id = 3, Title = "Eiffel Tower", GuideId = 2 },
             new { Id = 4, Title = "Tour 4", GuideId = 1 }
        );

        modelBuilder.Entity<Traveler>().HasData(
            new Traveler { Id = 1, FullName = "John Doe" }
        );
        
        modelBuilder.Entity<Passport>().HasData(
            new { Id = 1, PassportNumber = "123456", TravelerId = 1 }
        );
    }
}