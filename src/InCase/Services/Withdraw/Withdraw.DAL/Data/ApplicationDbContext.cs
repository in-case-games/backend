using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameItem> Items => Set<GameItem>();
        public DbSet<GameMarket> Markets => Set<GameMarket>();
        public DbSet<WithdrawStatus> Statuses => Set<WithdrawStatus>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserHistoryWithdraw> Withdraws => Set<UserHistoryWithdraw>();
        public DbSet<UserInventory> Inventories => Set<UserInventory>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
