using Microsoft.EntityFrameworkCore;
using RetailStoreAPI.Models;

namespace RetailStoreAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Discount> Discounts => Set<Discount>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        // Discount
        modelBuilder.Entity<Discount>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.Code).IsRequired().HasMaxLength(20);
            e.HasIndex(d => d.Code).IsUnique();
            e.Property(d => d.Percentage).IsRequired();
        });

        // Product
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(150);
            e.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.Discount)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Employee
        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(emp => emp.Id);
            e.Property(emp => emp.FirstName).IsRequired().HasMaxLength(100);
            e.Property(emp => emp.LastName).IsRequired().HasMaxLength(100);
            e.Property(emp => emp.Role).IsRequired();
        });

        // --- Seed Data ---

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Gadgets and electronic devices" },
            new Category { Id = 2, Name = "Clothing", Description = "Apparel and accessories" },
            new Category { Id = 3, Name = "Food", Description = "Groceries and consumables" },
            new Category { Id = 4, Name = "Sports", Description = "Sports equipment and gear" }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount { Id = 1, Code = "SUMMER10", Percentage = 10, ExpiresAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Discount { Id = 2, Code = "SALE25", Percentage = 25, ExpiresAt = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc) },
            new Discount { Id = 3, Code = "FLASH50", Percentage = 50, ExpiresAt = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Pro 15",      Price = 1299.99m, Stock = 20, CategoryId = 1, DiscountId = 1 },
            new Product { Id = 2, Name = "Wireless Earbuds",   Price = 89.99m,  Stock = 150, CategoryId = 1, DiscountId = null },
            new Product { Id = 3, Name = "Smart Watch",        Price = 249.99m, Stock = 60,  CategoryId = 1, DiscountId = 2 },
            new Product { Id = 4, Name = "Running Jacket",     Price = 69.99m,  Stock = 80,  CategoryId = 2, DiscountId = 3 },
            new Product { Id = 5, Name = "Classic T-Shirt",    Price = 24.99m,  Stock = 200, CategoryId = 2, DiscountId = null },
            new Product { Id = 6, Name = "Organic Coffee 1kg", Price = 15.49m,  Stock = 300, CategoryId = 3, DiscountId = null },
            new Product { Id = 7, Name = "Protein Bars (12x)", Price = 19.99m,  Stock = 180, CategoryId = 3, DiscountId = 1 },
            new Product { Id = 8, Name = "Tennis Racket",      Price = 119.99m, Stock = 45,  CategoryId = 4, DiscountId = null },
            new Product { Id = 9, Name = "Yoga Mat",           Price = 39.99m,  Stock = 90,  CategoryId = 4, DiscountId = 2 }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "Alice",   LastName = "Johnson", Role = "Manager",   HiredAt = new DateTime(2020, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Employee { Id = 2, FirstName = "Bob",     LastName = "Smith",   Role = "Cashier",   HiredAt = new DateTime(2022, 7, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Employee { Id = 3, FirstName = "Carol",   LastName = "Davis",   Role = "Stock",     HiredAt = new DateTime(2021, 11, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Employee { Id = 4, FirstName = "David",   LastName = "Wilson",  Role = "Cashier",   HiredAt = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
