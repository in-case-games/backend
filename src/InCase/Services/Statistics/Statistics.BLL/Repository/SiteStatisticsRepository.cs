using MongoDB.Driver;
using Statistics.DAL.Entities;

namespace Statistics.BLL.Repository
{
    public class SiteStatisticsRepository : ISiteStatisticsRepository
    {
        private readonly IMongoCollection<SiteStatistics> _siteStatistics;
        private readonly IMongoCollection<SiteStatisticsAdmin> _siteStatisticsAdmin;

        public SiteStatisticsRepository(IMongoClient client)
        {
            var database = client.GetDatabase("InCaseStatistics");

            _siteStatistics = database.GetCollection<SiteStatistics>("Site");
            _siteStatisticsAdmin = database.GetCollection<SiteStatisticsAdmin>("AdminSite");
        }

        [Obsolete("Obsolete")]
        public async Task<SiteStatistics> GetAsync(CancellationToken cancellation = default)
        {
            var statistics = await _siteStatistics.Find("{}").FirstOrDefaultAsync(cancellation);

            if (statistics is null) await _siteStatistics.InsertOneAsync(new SiteStatistics(), cancellation);

            return statistics ?? new SiteStatistics();
        }

        [Obsolete("Obsolete")]
        public async Task<SiteStatisticsAdmin> GetAdminAsync(CancellationToken cancellation = default)
        {
            var statistics = await _siteStatisticsAdmin.Find("{}").FirstOrDefaultAsync(cancellation);

            if (statistics is null) await _siteStatisticsAdmin.InsertOneAsync(new SiteStatisticsAdmin(), cancellation);

            return statistics ?? new SiteStatisticsAdmin();
        }
    }
}
