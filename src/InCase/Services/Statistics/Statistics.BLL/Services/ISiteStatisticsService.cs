using Statistics.BLL.Models;

namespace Statistics.BLL.Services;

public interface ISiteStatisticsService
{
    public Task<SiteStatisticsResponse> GetAsync(CancellationToken cancellation = default);
    public Task<SiteStatisticsAdminResponse> GetAdminAsync(CancellationToken cancellation = default);
}