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
            var database = client.GetDatabase("InCaseStatistics");

            _siteStatisticsAdmin = database
                .GetCollection<SiteStatisticsAdmin>("AdminSite");

            _siteStatisticsRepository = siteStatisticsRepository;
        }

        public async Task Consume(ConsumeContext<SiteStatisticsAdminTemplate> context)
        {
            var template = context.Message;

            var stat = await _siteStatisticsRepository.GetAdminAsync();
            var statNew = stat.ToJoin(template);

            var filter = Builders<SiteStatisticsAdmin>.Filter.Eq(s => s.Id, stat.Id);

            await _siteStatisticsAdmin.ReplaceOneAsync(filter, statNew);
        }
    }
}
