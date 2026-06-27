using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Migrations are stored in ProductApi.Infrastructure

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product table - matches assessment schema exactly
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "dbo");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).UseIdentityColumn();
            entity.Property(p => p.ProductName).IsRequired().HasMaxLength(255);
            entity.Property(p => p.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ModifiedBy).HasMaxLength(100);
            entity.Property(p => p.CreatedOn).IsRequired();
            entity.HasMany(p => p.Items)
                  .WithOne(i => i.Product)
                  .HasForeignKey(i => i.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Item table - matches assessment schema exactly
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item", "dbo");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).UseIdentityColumn();
            entity.Property(i => i.Quantity).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
