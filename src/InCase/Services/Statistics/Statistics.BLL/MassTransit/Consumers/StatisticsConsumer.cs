using Infrastructure.MassTransit.Statistics;
using MassTransit;
using MongoDB.Driver;
using Statistics.BLL.Repository;
using Statistics.DAL.Entities;

namespace Statistics.BLL.MassTransit.Consumers;
public class StatisticsConsumer : IConsumer<SiteStatisticsTemplate>
{
    private readonly IMongoCollection<SiteStatistics> _siteStatistics;
    private readonly ISiteStatisticsRepository _siteStatisticsRepository;

    public StatisticsConsumer(IMongoClient client, ISiteStatisticsRepository siteStatisticsRepository)
    {
        var database = client.GetDatabase("InCaseStatistics");

        _siteStatistics = database.GetCollection<SiteStatistics>("Site");
        _siteStatisticsRepository = siteStatisticsRepository;
    }

    public async Task Consume(ConsumeContext<SiteStatisticsTemplate> context)
    {
        var stats = await _siteStatisticsRepository.GetAsync();
        var statsNew = new SiteStatistics
        {
            Id = stats.Id,
            LootBoxes = stats.LootBoxes + context.Message.LootBoxes,
            Reviews = stats.Reviews + context.Message.Reviews,
            Users = stats.Users + context.Message.Users,
            WithdrawnFunds = stats.WithdrawnFunds + context.Message.WithdrawnFunds,
            WithdrawnItems = stats.WithdrawnItems + context.Message.WithdrawnItems
        };

        var filter = Builders<SiteStatistics>.Filter.Eq(s => s.Id, stats.Id);

        await _siteStatistics.ReplaceOneAsync(filter, statsNew);
    }
}