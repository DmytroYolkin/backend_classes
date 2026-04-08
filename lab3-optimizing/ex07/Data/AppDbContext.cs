using Microsoft.EntityFrameworkCore;
using ex07.Models;

namespace ex07.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Workout> Workouts => Set<Workout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Workout>()
            .HasKey(w => w.Id);

        modelBuilder.Entity<Workout>()
            .HasOne(w => w.Exercise)
            .WithMany()
            .HasForeignKey(w => w.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
