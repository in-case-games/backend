using Statistics.DAL.Entities;

namespace Statistics.BLL.Repository
{
    public interface ISiteStatisticsRepository
    {
        public Task<SiteStatistics> GetAsync(CancellationToken cancellation = default);
        public Task<SiteStatisticsAdmin> GetAdminAsync(CancellationToken cancellation = default);
    }
}
