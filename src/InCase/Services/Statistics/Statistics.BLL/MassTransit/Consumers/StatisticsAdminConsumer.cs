using Infrastructure.MassTransit.Statistics;
using MassTransit;
using MongoDB.Driver;
using Statistics.BLL.Helpers;
using Statistics.BLL.Repository;
using Statistics.DAL.Entities;

namespace Statistics.BLL.MassTransit.Consumers
{
    public class StatisticsAdminConsumer : IConsumer<SiteStatisticsAdminTemplate>
    {
        private readonly IMongoCollection<SiteStatisticsAdmin> _siteStatisticsAdmin;
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;

        public StatisticsAdminConsumer(
            IMongoClient client,
            ISiteStatisticsRepository siteStatisticsRepository)
        {
            IMongoDatabase database = client.GetDatabase("InCaseStatistics");

            _siteStatisticsAdmin = database
                .GetCollection<SiteStatisticsAdmin>("AdminSite");

            _siteStatisticsRepository = siteStatisticsRepository;
        }

        public async Task Consume(ConsumeContext<SiteStatisticsAdminTemplate> context)
        {
            SiteStatisticsAdminTemplate template = context.Message;

            SiteStatisticsAdmin statistics = await _siteStatisticsRepository.GetAdminAsync();
            SiteStatisticsAdmin statisticsNew = statistics.ToJoin(template);

            var filter = Builders<SiteStatisticsAdmin>.Filter.Eq(s => s.Id, statistics.Id);

            await _siteStatisticsAdmin.ReplaceOneAsync(filter, statisticsNew);
        }
    }
}
