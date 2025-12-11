using System;
using LendingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)  : base(options) {}

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>()
            .Property(l => l.PrincipalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Loan>()
            .Property(l => l.InterestRate)
            .HasPrecision(3, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Loan)
            .WithMany(l => l.Payments)
            .HasForeignKey(p => p.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
