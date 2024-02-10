using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Game.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GameItem> GameItems => Set<GameItem>();
    public DbSet<LootBox> LootBoxes => Set<LootBox>();
    public DbSet<LootBoxInventory> LootBoxInventories => Set<LootBoxInventory>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
    public DbSet<UserOpening> UserOpenings => Set<UserOpening>();
    public DbSet<UserPromoCode> UserPromoCodes => Set<UserPromoCode>();
    public DbSet<UserPathBanner> UserPathBanners => Set<UserPathBanner>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}