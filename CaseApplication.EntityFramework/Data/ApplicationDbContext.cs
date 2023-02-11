using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CaseApplication.EntityFramework.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal DbSet<User> User => Set<User>();
        internal DbSet<UserRole> UserRole => Set<UserRole>();
        internal DbSet<Promocode> Promocode => Set<Promocode>();
        internal DbSet<PromocodesUsedByUser> PromocodeUsedByUsers => Set<PromocodesUsedByUser>();
        internal DbSet<PromocodeType> PromocodeType => Set<PromocodeType>();
        internal DbSet<UserRestriction> UserRestriction => Set<UserRestriction>();
        internal DbSet<UserAdditionalInfo> UserAdditionalInfo => Set<UserAdditionalInfo>();
        internal DbSet<UserInventory> UserInventory => Set<UserInventory>();
        internal DbSet<GameCase> GameCase => Set<GameCase>();
        internal DbSet<GameItem> GameItem => Set<GameItem>();
        internal DbSet<SiteStatistics> SiteStatistics => Set<SiteStatistics>();
        internal DbSet<CaseInventory> CaseInventory => Set<CaseInventory>();
        internal DbSet<UserHistoryOpeningCases> UserHistoryOpeningCases => Set<UserHistoryOpeningCases>();
        internal DbSet<News> News => Set<News>();
        internal DbSet<UserToken> UserToken => Set<UserToken>();

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
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
