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

        public async Task<SiteStatisticsResponse> GetAsync()
        {
            var stats = await _siteStatisticsRepository.GetAsync();

            return stats.ToResponse();
        }

        public async Task<SiteStatisticsAdminResponse> GetAdminAsync()
        {
            var stats = await _siteStatisticsRepository.GetAdminAsync();

            return stats.ToResponse();
        }
    }
}
