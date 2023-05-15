using InCase.Infrastructure.Models.Statistics.Response;
using Statistics.DAL.Entities;

namespace Statistics.DAL.Helpers
{
    public static class SiteStatisticsTransformers
    {
        public static SiteStatisticsResponse ToResponse(this SiteStatistics stats)
        {
            return new() { 
                LootBoxes = stats.LootBoxes,
                Reviews = stats.Reviews,
                Users = stats.Users,
                WithdrawnFunds = stats.WithdrawnFunds,
                WithdrawnItems = stats.WithdrawnItems
            };
        }

        public static SiteStatisticsAdminResponse ToResponse(this SiteStatisticsAdmin stats)
        {
            return new()
            {
                BalanceWithdrawn = stats.BalanceWithdrawn,
                SentSites = stats.SentSites,
                TotalReplenished = stats.TotalReplenished,
            };
        }
    }
}
