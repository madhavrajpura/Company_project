using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.CreatedAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Product>().Property(p => p.UpdatedAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Product>().Property(p => p.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<Category>().Property(p => p.CreatedAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Category>().Property(p => p.UpdatedAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Category>().Property(p => p.IsActive).HasDefaultValue(true);

    }


}
