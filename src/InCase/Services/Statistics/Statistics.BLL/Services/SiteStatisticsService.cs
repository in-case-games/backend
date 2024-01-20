using Statistics.BLL.Models;
using Statistics.BLL.Repository;

namespace Statistics.BLL.Services
{
    public class SiteStatisticsService(ISiteStatisticsRepository siteStatisticsRepository) : ISiteStatisticsService
    {
        public async Task<SiteStatisticsResponse> GetAsync(CancellationToken cancellation = default)
        {
            var stats = await siteStatisticsRepository.GetAsync(cancellation);

            return new SiteStatisticsResponse
            {
                LootBoxes = stats.LootBoxes,
                Reviews = stats.Reviews,
                Users = stats.Users,
                WithdrawnFunds = stats.WithdrawnFunds,
                WithdrawnItems = stats.WithdrawnItems
            };
        }

        public async Task<SiteStatisticsAdminResponse> GetAdminAsync(CancellationToken cancellation = default)
        {
            var stats = await siteStatisticsRepository.GetAdminAsync(cancellation);

            return new SiteStatisticsAdminResponse
            {
                FundsUsersInventories = stats.FundsUsersInventories,
                ReturnedFunds = stats.ReturnedFunds,
                TotalReplenishedFunds = stats.TotalReplenishedFunds,
                RevenueLootBoxCommission = stats.RevenueLootBoxCommission,
            };
        }
    }
}
