using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Configurations;

namespace CaseApplication.EntityFramework.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal DbSet<User> User => Set<User>();
        internal DbSet<UserRole> UserRole => Set<UserRole>();
        internal DbSet<UserRestriction> UserRestriction => Set<UserRestriction>();
        internal DbSet<UserAdditionalInfo> UserAdditionalInfo => Set<UserAdditionalInfo>();
        internal DbSet<UserInventory> UserInventory => Set<UserInventory>();
        internal DbSet<GameCase> GameCase => Set<GameCase>();
        internal DbSet<GameItem> GameItem => Set<GameItem>();
        internal DbSet<SiteStatistics> SiteStatistics => Set<SiteStatistics>();
        internal DbSet<CaseInventory> CaseInventory => Set<CaseInventory>();
        internal DbSet<UserHistoryOpeningCases> UserHistoryOpeningCases => Set<UserHistoryOpeningCases>();
        internal DbSet<News> News => Set<News>();

        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CaseInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new GameCaseConfiguration());
            modelBuilder.ApplyConfiguration(new GameItemConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new UserAdditionalInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserRestrictionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserHistoryOpeningCasesConfiguration());
            modelBuilder.ApplyConfiguration(new SiteStatisticsConfiguration());
        }
    }
}
