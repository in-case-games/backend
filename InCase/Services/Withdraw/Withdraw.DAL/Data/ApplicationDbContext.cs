using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameItem> GameItems => Set<GameItem>();
        public DbSet<GameMarket> GameMarkets => Set<GameMarket>();
        public DbSet<WithdrawStatus> WithdrawStatuses => Set<WithdrawStatus>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserHistoryWithdraw> HistoryWithdraws => Set<UserHistoryWithdraw>();
        public DbSet<UserInventory> UserInventories => Set<UserInventory>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
