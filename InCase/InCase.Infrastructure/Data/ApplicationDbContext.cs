using Microsoft.EntityFrameworkCore;
using InCase.Infrastructure.Configurations;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AnswerImage> AnswerImages => Set<AnswerImage>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameItem> GameItems => Set<GameItem>();
        public DbSet<GameItemQuality> GameItemQualities => Set<GameItemQuality>();
        public DbSet<GameItemRarity> GameItemRarities => Set<GameItemRarity>();
        public DbSet<GameItemType> GameItemTypes => Set<GameItemType>();
        public DbSet<GamePlatform> GamePlatforms => Set<GamePlatform>();
        public DbSet<GroupLootBox> GroupLootBoxes => Set<GroupLootBox>();
        public DbSet<LootBox> LootBoxes => Set<LootBox>();
        public DbSet<LootBoxBanner> LootBoxBanners => Set<LootBoxBanner>();
        public DbSet<LootBoxGroup> LootBoxGroups => Set<LootBoxGroup>();
        public DbSet<LootBoxInventory> LootBoxInventories => Set<LootBoxInventory>();
        public DbSet<News> News => Set<News>();
        public DbSet<NewsImage> NewsImages => Set<NewsImage>();
        public DbSet<Promocode> Promocodes => Set<Promocode>();
        public DbSet<PromocodeType> PromocodeTypes => Set<PromocodeType>();
        public DbSet<RestrictionType> RestrictionTypes => Set<RestrictionType>();
        public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();
        public DbSet<SiteStatitics> SiteStatitics => Set<SiteStatitics>();
        public DbSet<SiteStatiticsAdmin> SiteStatiticsAdmins => Set<SiteStatiticsAdmin>();
        public DbSet<SupportTopic> SupportTopics => Set<SupportTopic>();
        public DbSet<SupportTopicAnswer> SupportTopicAnswers => Set<SupportTopicAnswer>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserHistoryOpening> UserHistoryOpenings => Set<UserHistoryOpening>();
        public DbSet<UserHistoryPayment> UserHistoryPayments => Set<UserHistoryPayment>();
        public DbSet<UserHistoryPromocode> UserHistoryPromocodes => Set<UserHistoryPromocode>();
        public DbSet<UserHistoryWithdrawn> UserHistoryWithdrawns => Set<UserHistoryWithdrawn>();
        public DbSet<UserInventory> UserInventories => Set<UserInventory>();
        public DbSet<UserPathBanner> UserPathBanners => Set<UserPathBanner>();
        public DbSet<UserRestriction> UserRestrictions => Set<UserRestriction>();
        public DbSet<UserReview> UserReviews => Set<UserReview>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<UserToken> UserTokens => Set<UserToken>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLowerCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region .ApplyConfiguration()
            modelBuilder.ApplyConfiguration(new AnswerImageConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GameItemConfiguration());
            modelBuilder.ApplyConfiguration(new GameItemQualityConfiguration());
            modelBuilder.ApplyConfiguration(new GameItemRarityConfiguration());
            modelBuilder.ApplyConfiguration(new GameItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GamePlatformConfiguration());
            modelBuilder.ApplyConfiguration(new GroupLootBoxConfiguration());
            modelBuilder.ApplyConfiguration(new LootBoxConfiguration());
            modelBuilder.ApplyConfiguration(new LootBoxBannerConfiguration());
            modelBuilder.ApplyConfiguration(new LootBoxGroupConfiguration());
            modelBuilder.ApplyConfiguration(new LootBoxInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new NewsImageConfiguration());
            modelBuilder.ApplyConfiguration(new PromocodeConfiguration());
            modelBuilder.ApplyConfiguration(new PromocodeTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RestrictionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewImageConfiguration());
            modelBuilder.ApplyConfiguration(new SiteStatiticsConfiguration());
            modelBuilder.ApplyConfiguration(new SiteStatiticsAdminConfiguration());
            modelBuilder.ApplyConfiguration(new SupportTopicConfiguration());
            modelBuilder.ApplyConfiguration(new SupportTopicAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserAdditionalInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserHistoryOpeningConfiguration());
            modelBuilder.ApplyConfiguration(new UserHistoryPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new UserHistoryPromocodeConfiguration());
            modelBuilder.ApplyConfiguration(new UserHistoryWithdrawnConfiguration());
            modelBuilder.ApplyConfiguration(new UserInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserPathBannerConfiguration());
            modelBuilder.ApplyConfiguration(new UserRestrictionConfiguration());
            modelBuilder.ApplyConfiguration(new UserReviewConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
            #endregion

            #region .HasData()
            #endregion
        }
    }
}
