using Statistics.BLL.Helpers;
using Statistics.BLL.Models;
using Statistics.BLL.Repository;

namespace Statistics.BLL.Services
{
    public class SiteStatisticsService : ISiteStatisticsService
    {
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;

        public SiteStatisticsService(ISiteStatisticsRepository siteStatisticsRepository)
        {
            _siteStatisticsRepository = siteStatisticsRepository;
        }

        public async Task<SiteStatisticsResponse> GetAsync(CancellationToken cancellation = default)
        {
            var stats = await _siteStatisticsRepository.GetAsync(cancellation);

            return stats.ToResponse();
        }

        public async Task<SiteStatisticsAdminResponse> GetAdminAsync(CancellationToken cancellation = default)
        {
            var stats = await _siteStatisticsRepository.GetAdminAsync(cancellation);

            return stats.ToResponse();
        }
    }
}
