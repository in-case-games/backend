using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ISiteStatisticsRepository
    {
        public Task<SiteStatistics?> Get();
    }
}
