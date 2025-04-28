using Ab.Inbev.Service.Domain.Employees.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var employee = modelBuilder.Entity<Employee>();

        employee.HasKey(e => e.Id);

        employee.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        employee.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        employee.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        employee.HasIndex(e => e.Email)
            .IsUnique();

        employee.Property(e => e.DocumentNumber)
            .IsRequired()
            .HasMaxLength(50);

        employee.HasIndex(e => e.DocumentNumber)
            .IsUnique();

        employee.Property(e => e.PasswordHash)
            .IsRequired();

        employee.HasOne(e => e.Manager)
            .WithMany(m => m.Subordinates)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        employee.HasMany(e => e.PhoneNumbers)
            .WithOne(p => p.Employee)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        var phone = modelBuilder.Entity<PhoneNumber>();
        phone.HasKey(p => p.Id);
        phone.Property(p => p.Number)
            .IsRequired()
            .HasMaxLength(20);
    }
}
