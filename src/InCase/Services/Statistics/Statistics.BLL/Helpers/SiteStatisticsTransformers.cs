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
                FundsUsersInventories = stats.FundsUsersInventories,
                ReturnedFunds = stats.ReturnedFunds,
                TotalReplenishedFunds = stats.TotalReplenishedFunds,
                RevenueLootBoxCommission = stats.RevenueLootBoxCommission,
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
                FundsUsersInventories = entity.FundsUsersInventories + template.FundsUsersInventories,
                ReturnedFunds = entity.ReturnedFunds + template.ReturnedFunds,
                TotalReplenishedFunds = entity.TotalReplenishedFunds + template.TotalReplenishedFunds,
                RevenueLootBoxCommission = entity.RevenueLootBoxCommission + template.RevenueLootBoxCommission,
            };
    }
}
