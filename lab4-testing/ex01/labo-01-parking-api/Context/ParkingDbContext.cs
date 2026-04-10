namespace labo_01_parking_api.Context;

public class ParkingDbContext(DbContextOptions<ParkingDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Brand).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Plate).IsRequired().HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Color).IsRequired().HasMaxLength(30);
            entity.HasIndex(e => e.Plate).IsUnique();
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Plate).IsRequired().HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Start).IsRequired();
            entity.Property(e => e.End);
            entity.Property(e => e.CarId).IsRequired();
            entity.Property(e => e.TotalPrice).HasPrecision(8, 2);
            entity.Property(e => e.IsFinished).IsRequired().HasDefaultValue(false);
            entity.HasOne<Car>().WithMany().HasForeignKey(e => e.CarId);
        });
    }
}
