using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Game.DAL.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<GameItem> Items => Set<GameItem>();
        public DbSet<LootBox> Boxes => Set<LootBox>();
        public DbSet<LootBoxInventory> BoxInventories => Set<LootBoxInventory>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserOpening> Openings => Set<UserOpening>();
        public DbSet<UserPromocode> UserPromocodes => Set<UserPromocode>();
        public DbSet<UserPathBanner> PathBanners => Set<UserPathBanner>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
