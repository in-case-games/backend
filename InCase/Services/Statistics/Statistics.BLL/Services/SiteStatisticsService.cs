using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Statistics.DAL.Entities;

namespace Statistics.BLL.Services
{
    public class SiteStatisticsService
    {
        private readonly IMongoCollection<SiteStatistics> _statistics;
        private readonly IMongoCollection<SiteStatisticsAdmin> _statisticsAdmin;

        public SiteStatisticsService(IConfiguration configuration)
        {
            MongoClient client = new(
                configuration["SiteStatisticsDB:ConnectionString"]);
            MongoClient clientAdmin = new(
                configuration["SiteStatisticsAdminDB:ConnectionString"]);

            IMongoDatabase db = client.GetDatabase(
                configuration["SiteStatisticsDB:DatabaseName"]);
            IMongoDatabase dbAdmin = clientAdmin.GetDatabase(
                configuration["SiteStatisticsAdminDB:DatabaseName"]);

            _statistics = db.GetCollection<SiteStatistics>(
                configuration["SiteStatisticsDB:CollectionName"]);
            _statisticsAdmin = dbAdmin.GetCollection<SiteStatisticsAdmin>(
                configuration["SiteStatisticsAdminDB:CollectionName"]);
        }

        public async Task<SiteStatistics> Get()
        {
            SiteStatistics? statistics = await _statistics
                .Find("{}")
                .FirstOrDefaultAsync();

            if(statistics is null)
                await _statistics.InsertOneAsync(new SiteStatistics());

            return statistics ?? new();
        }

        public async Task<SiteStatisticsAdmin> GetAdmin()
        {
            SiteStatisticsAdmin? statistics = await _statisticsAdmin
                .Find("{}")
                .FirstOrDefaultAsync();

            if (statistics is null)
                await _statisticsAdmin.InsertOneAsync(new SiteStatisticsAdmin());

            return statistics ?? new();
        }
    }
}
