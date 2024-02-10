using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Entities;
using System.Reflection;

namespace Promocode.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<PromoCodeType> PromoCodesTypes => Set<PromoCodeType>();
    public DbSet<UserPromoCode> UserPromoCodes => Set<UserPromoCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}