using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class SiteStatisticsRepository : ISiteStatisticsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SiteStatisticsRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SiteStatistics> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SiteStatistics? siteStatistics = await context
                .SiteStatistics
                .FirstOrDefaultAsync();

            return siteStatistics ?? throw new("There is no such statistics in the database, " +
                "review what data comes from the api");
        }
    }
}
