using Infrastructure.MassTransit.Statistics;
using MassTransit;
using MongoDB.Driver;
using Statistics.BLL.Repository;
using Statistics.DAL.Entities;

namespace Statistics.BLL.MassTransit.Consumers;
public class StatisticsAdminConsumer : IConsumer<SiteStatisticsAdminTemplate>
{
    private readonly IMongoCollection<SiteStatisticsAdmin> _siteStatisticsAdmin;
    private readonly ISiteStatisticsRepository _siteStatisticsRepository;

    public StatisticsAdminConsumer(IMongoClient client, ISiteStatisticsRepository siteStatisticsRepository)
    {
        var database = client.GetDatabase("InCaseStatistics");

        _siteStatisticsAdmin = database.GetCollection<SiteStatisticsAdmin>("AdminSite");
        _siteStatisticsRepository = siteStatisticsRepository;
    }

    public async Task Consume(ConsumeContext<SiteStatisticsAdminTemplate> context)
    {
        var stats = await _siteStatisticsRepository.GetAdminAsync();
        var statsNew = new SiteStatisticsAdmin
        {
            Id = stats.Id,
            FundsUsersInventories = stats.FundsUsersInventories + context.Message.FundsUsersInventories,
            ReturnedFunds = stats.ReturnedFunds + context.Message.ReturnedFunds,
            TotalReplenishedFunds = stats.TotalReplenishedFunds + context.Message.TotalReplenishedFunds,
            RevenueLootBoxCommission = stats.RevenueLootBoxCommission + context.Message.RevenueLootBoxCommission,
        };

        var filter = Builders<SiteStatisticsAdmin>.Filter.Eq(s => s.Id, stats.Id);

        await _siteStatisticsAdmin.ReplaceOneAsync(filter, statsNew);
    }
}