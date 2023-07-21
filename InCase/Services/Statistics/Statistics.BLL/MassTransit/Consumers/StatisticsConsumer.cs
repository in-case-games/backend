using Infrastructure.MassTransit.Statistics;
using MassTransit;
using MongoDB.Driver;
using Statistics.BLL.Helpers;
using Statistics.BLL.Repository;
using Statistics.DAL.Entities;

namespace Statistics.BLL.MassTransit.Consumers
{
    public class StatisticsConsumer : IConsumer<SiteStatisticsTemplate>
    {
        private readonly IMongoCollection<SiteStatistics> _siteStatistics;
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;

        public StatisticsConsumer(
            IMongoClient client,
            ISiteStatisticsRepository siteStatisticsRepository)
        {
            IMongoDatabase database = client.GetDatabase("InCaseStatistics");

            _siteStatistics = database
                .GetCollection<SiteStatistics>("Site");

            _siteStatisticsRepository = siteStatisticsRepository;
        }

        public async Task Consume(ConsumeContext<SiteStatisticsTemplate> context)
        {
            SiteStatisticsTemplate template = context.Message;

            SiteStatistics statistics = await _siteStatisticsRepository.GetAsync();
            SiteStatistics statisticsNew = statistics.ToJoin(template);

            var filter = Builders<SiteStatistics>.Filter.Eq(s => s.Id, statistics.Id);

            await _siteStatistics.ReplaceOneAsync(filter, statisticsNew);
        }
    }
}
