using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameItem> GameItems => Set<GameItem>();
    public DbSet<GameMarket> GameMarkets => Set<GameMarket>();
    public DbSet<WithdrawStatus> WithdrawStatuses => Set<WithdrawStatus>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserHistoryWithdraw> UserHistoryWithdraws => Set<UserHistoryWithdraw>();
    public DbSet<UserInventory> UserInventories => Set<UserInventory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}