using Microsoft.EntityFrameworkCore;
using Resources.DAL.Entities;
using System.Reflection;

namespace Resources.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameItem> GameItems => Set<GameItem>();
    public DbSet<GameItemQuality> GameItemQualities => Set<GameItemQuality>();
    public DbSet<GameItemRarity> GameItemRarities => Set<GameItemRarity>();
    public DbSet<GameItemType> GameItemTypes => Set<GameItemType>();
    public DbSet<GroupLootBox> GroupLootBoxes => Set<GroupLootBox>();
    public DbSet<LootBox> LootBoxes => Set<LootBox>();
    public DbSet<LootBoxBanner> LootBoxBanners => Set<LootBoxBanner>();
    public DbSet<LootBoxGroup> LootBoxGroups => Set<LootBoxGroup>();
    public DbSet<LootBoxInventory> LootBoxInventories => Set<LootBoxInventory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}