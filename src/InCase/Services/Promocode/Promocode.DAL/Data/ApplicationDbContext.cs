using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Entities;
using System.Reflection;

namespace Promocode.DAL.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<PromocodeEntity> Promocodes => Set<PromocodeEntity>();
        public DbSet<PromocodeType> PromocodesTypes => Set<PromocodeType>();
        public DbSet<UserPromocode> UserPromocodes => Set<UserPromocode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
