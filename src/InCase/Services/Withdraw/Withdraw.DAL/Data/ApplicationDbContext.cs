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

        foreach (var gp in _gamePlatform)
        {
            Game game = new() { Name = gp.Key };
            GameMarket market = new() { Name = gp.Value, GameId = game.Id };

            modelBuilder.Entity<Game>().HasData(game);
            modelBuilder.Entity<GameMarket>().HasData(market);
        }
    }

    private readonly Dictionary<string, string> _gamePlatform = new()
    {
        ["csgo"] = "tm",
        ["dota2"] = "tm"
    };
}