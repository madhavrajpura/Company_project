using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Attendence> Attendences { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendence>().Property(a => a.Date).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Attendence>().Property(a => a.CheckInTime).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Attendence>().Property(a => a.CheckOutTime).HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Employee>().Property(e => e.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<Attendence>().Property(a => a.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
    }
}
