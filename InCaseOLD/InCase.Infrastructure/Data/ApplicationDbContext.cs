﻿using Microsoft.EntityFrameworkCore;
using InCase.Domain.Entities.Resources;
using System.Reflection;

namespace InCase.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region IncludedEntities (DbSet)
        public DbSet<AnswerImage> AnswerImages => Set<AnswerImage>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameItem> GameItems => Set<GameItem>();
        public DbSet<GameItemQuality> GameItemQualities => Set<GameItemQuality>();
        public DbSet<GameItemRarity> GameItemRarities => Set<GameItemRarity>();
        public DbSet<GameItemType> GameItemTypes => Set<GameItemType>();
        public DbSet<GameMarket> GameMarkets => Set<GameMarket>();
        public DbSet<GroupLootBox> GroupLootBoxes => Set<GroupLootBox>();
        public DbSet<LootBox> LootBoxes => Set<LootBox>();
        public DbSet<LootBoxBanner> LootBoxBanners => Set<LootBoxBanner>();
        public DbSet<LootBoxGroup> LootBoxGroups => Set<LootBoxGroup>();
        public DbSet<LootBoxInventory> LootBoxInventories => Set<LootBoxInventory>();
        public DbSet<Promocode> Promocodes => Set<Promocode>();
        public DbSet<PromocodeType> PromocodeTypes => Set<PromocodeType>();
        public DbSet<RestrictionType> RestrictionTypes => Set<RestrictionType>();
        public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();
        public DbSet<SiteStatistics> SiteStatistics => Set<SiteStatistics>();
        public DbSet<SiteStatisticsAdmin> SiteStatisticsAdmins => Set<SiteStatisticsAdmin>();
        public DbSet<SupportTopic> SupportTopics => Set<SupportTopic>();
        public DbSet<SupportTopicAnswer> SupportTopicAnswers => Set<SupportTopicAnswer>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserHistoryOpening> UserHistoryOpenings => Set<UserHistoryOpening>();
        public DbSet<UserHistoryPayment> UserHistoryPayments => Set<UserHistoryPayment>();
        public DbSet<InvoicePaymentStatus> InvoicePaymentStatuses => Set<InvoicePaymentStatus>();
        public DbSet<UserHistoryPromocode> UserHistoryPromocodes => Set<UserHistoryPromocode>();
        public DbSet<UserHistoryWithdraw> UserHistoryWithdraws => Set<UserHistoryWithdraw>();
        public DbSet<ItemWithdrawStatus> ItemWithdrawStatuses => Set<ItemWithdrawStatus>();
        public DbSet<UserInventory> UserInventories => Set<UserInventory>();
        public DbSet<UserPathBanner> UserPathBanners => Set<UserPathBanner>();
        public DbSet<UserRestriction> UserRestrictions => Set<UserRestriction>();
        public DbSet<UserReview> UserReviews => Set<UserReview>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        #endregion
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}