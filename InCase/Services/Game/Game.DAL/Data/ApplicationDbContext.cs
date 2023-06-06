using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Game.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GameItem> GameItems => Set<GameItem>();
        public DbSet<LootBox> Boxes => Set<LootBox>();
        public DbSet<LootBoxInventory> BoxInventories => Set<LootBoxInventory>();
        public DbSet<Promocode> Promocodes => Set<Promocode>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserHistoryOpening> HistoryOpenings => Set<UserHistoryOpening>();
        public DbSet<UserHistoryPromocode> HistoryPromocodes => Set<UserHistoryPromocode>();
        public DbSet<UserPathBanner> PathBanners => Set<UserPathBanner>();

        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
