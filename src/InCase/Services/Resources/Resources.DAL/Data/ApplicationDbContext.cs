using Microsoft.EntityFrameworkCore;
using Resources.DAL.Entities;
using System.Reflection;

namespace Resources.DAL.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameItem> Items => Set<GameItem>();
        public DbSet<GameItemQuality> Qualities => Set<GameItemQuality>();
        public DbSet<GameItemRarity> Rarities => Set<GameItemRarity>();
        public DbSet<GameItemType> ItemTypes => Set<GameItemType>();
        public DbSet<GroupLootBox> GroupBoxes => Set<GroupLootBox>();
        public DbSet<LootBox> LootBoxes => Set<LootBox>();
        public DbSet<LootBoxBanner> Banners => Set<LootBoxBanner>();
        public DbSet<LootBoxGroup> Groups => Set<LootBoxGroup>();
        public DbSet<LootBoxInventory> BoxInventories => Set<LootBoxInventory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
