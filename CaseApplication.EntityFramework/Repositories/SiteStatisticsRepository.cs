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
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            SiteStatistics? siteStatistics = await _context
                .SiteStatistics
                .FirstAsync();

            return siteStatistics;
                
        }
    }
}
