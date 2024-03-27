using Microsoft.EntityFrameworkCore;
using Payment.DAL.Entities;
using System.Reflection;

namespace Payment.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserPayment> UserPayments => Set<UserPayment>();
    public DbSet<UserPromoCode> UserPromoCodes => Set<UserPromoCode>();
    public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}