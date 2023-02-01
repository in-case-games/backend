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
        public async Task<SiteStatistics?> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.SiteStatistics.FirstOrDefaultAsync(); ;
        }
    }
}
