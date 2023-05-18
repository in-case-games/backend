using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Entities;
using System.Reflection;

namespace Promocode.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<PromocodeEntity> Promocodes => Set<PromocodeEntity>();
        public DbSet<PromocodeType> PromocodesTypes => Set<PromocodeType>();
        public DbSet<UserHistoryPromocode> UserHistoriesPromocodes => Set<UserHistoryPromocode>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
