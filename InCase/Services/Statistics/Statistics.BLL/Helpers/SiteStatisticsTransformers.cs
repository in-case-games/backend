using Infrastructure.MassTransit.Statistics;
using Statistics.BLL.Models;
using Statistics.DAL.Entities;

namespace Statistics.BLL.Helpers
{
    public static class SiteStatisticsTransformers
    {
        public static SiteStatisticsResponse ToResponse(this SiteStatistics stats) => 
            new() { 
                LootBoxes = stats.LootBoxes,
                Reviews = stats.Reviews,
                Users = stats.Users,
                WithdrawnFunds = stats.WithdrawnFunds,
                WithdrawnItems = stats.WithdrawnItems
            };
        public static SiteStatisticsAdminResponse ToResponse(this SiteStatisticsAdmin stats) =>
            new()
            {
                BalanceWithdrawn = stats.BalanceWithdrawn,
                SentSites = stats.SentSites,
                TotalReplenished = stats.TotalReplenished,
            };

        public static SiteStatistics ToJoin(
            this SiteStatistics entity, 
            SiteStatisticsTemplate template) => new()
        {
            Id = entity.Id,
            LootBoxes = entity.LootBoxes + template.LootBoxes,
            Reviews = entity.Reviews + template.Reviews,
            Users = entity.Users + template.Users,
            WithdrawnFunds = entity.WithdrawnFunds + template.WithdrawnFunds,
            WithdrawnItems = entity.WithdrawnItems + template.WithdrawnItems
        };

        public static SiteStatisticsAdmin ToJoin(
            this SiteStatisticsAdmin entity,
            SiteStatisticsAdminTemplate template) => new()
            {
                Id = entity.Id,
                BalanceWithdrawn = entity.BalanceWithdrawn + template.BalanceWithdrawn,
                SentSites = entity.SentSites + template.SentSites,
                TotalReplenished = entity.TotalReplenished + template.TotalReplenished,
            };
    }
}
