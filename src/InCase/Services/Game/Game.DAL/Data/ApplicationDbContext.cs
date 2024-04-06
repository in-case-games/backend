using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Game.DAL.Data;
public class ApplicationDbContext: DbContext
{
    public virtual DbSet<GameItem> GameItems => Set<GameItem>();
    public virtual DbSet<LootBox> LootBoxes => Set<LootBox>();
    public virtual DbSet<LootBoxInventory> LootBoxInventories => Set<LootBoxInventory>();
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
    public virtual DbSet<UserOpening> UserOpenings => Set<UserOpening>();
    public virtual DbSet<UserPromoCode> UserPromoCodes => Set<UserPromoCode>();
    public virtual DbSet<UserPathBanner> UserPathBanners => Set<UserPathBanner>();

    public ApplicationDbContext() {  }
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}