using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DtlApi.Models;

namespace DtlApi.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed initial data
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Books", Description = "Books and educational materials", IsActive = true, CreatedAt = DateTime.UtcNow }
        );

        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, CategoryId = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Product { Id = 2, Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, CategoryId = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Product { Id = 3, Name = "Programming Book", Description = "Learn C# programming", Price = 39.99m, CategoryId = 3, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}
