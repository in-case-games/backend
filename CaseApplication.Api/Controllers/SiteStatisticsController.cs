using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteStatisticsController : Controller
    {
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;
        public SiteStatisticsController(ISiteStatisticsRepository siteStatisticsRepository)
        {
            _siteStatisticsRepository = siteStatisticsRepository;
        }
        [HttpGet]
        public async Task<SiteStatistics> Get()
        {
            return await _siteStatisticsRepository.Get();
        }
    }
}
