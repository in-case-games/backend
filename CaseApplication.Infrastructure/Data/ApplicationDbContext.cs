using Microsoft.EntityFrameworkCore;
using CaseApplication.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CaseApplication.Domain.Entities.Internal;

namespace CaseApplication.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User => Set<User>();
        public DbSet<UserRole> UserRole => Set<UserRole>();
        public DbSet<Promocode> Promocode => Set<Promocode>();
        public DbSet<PromocodesUsedByUser> PromocodeUsedByUsers => Set<PromocodesUsedByUser>();
        public DbSet<PromocodeType> PromocodeType => Set<PromocodeType>();
        public DbSet<UserRestriction> UserRestriction => Set<UserRestriction>();
        public DbSet<UserAdditionalInfo> UserAdditionalInfo => Set<UserAdditionalInfo>();
        public DbSet<UserInventory> UserInventory => Set<UserInventory>();
        public DbSet<GameCase> GameCase => Set<GameCase>();
        public DbSet<GameItem> GameItem => Set<GameItem>();
        public DbSet<SiteStatistics> SiteStatistics => Set<SiteStatistics>();
        public DbSet<CaseInventory> CaseInventory => Set<CaseInventory>();
        public DbSet<UserHistoryOpeningCases> UserHistoryOpeningCases => Set<UserHistoryOpeningCases>();
        public DbSet<News> News => Set<News>();
        public DbSet<UserToken> UserToken => Set<UserToken>();

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            // Database.Migrate();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
        }
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
            modelBuilder.ApplyConfiguration(new PromocodeConfiguration());
            modelBuilder.ApplyConfiguration(new PromocodeTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PromocodesUsedByUserConfiguration());

            #region Модели, создаваемые при разработке
            UserRole userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                RoleName = "user"
            };
            UserRole adminRole = new UserRole
            {
                Id = Guid.NewGuid(),
                RoleName = "admin"
            };

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole[]
                {
                    userRole,
                    adminRole
                });

            PromocodeType promocodeBalance = new PromocodeType
            {
                Id = Guid.NewGuid(),
                PromocodeTypeName = "balance"
            };

            PromocodeType promocodeCase = new PromocodeType
            {
                Id = Guid.NewGuid(),
                PromocodeTypeName = "case"
            };

            modelBuilder.Entity<PromocodeType>().HasData(
               new PromocodeType[]
               {
                    promocodeBalance,
                    promocodeCase
               });
            #endregion
        }
    }
}
