using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;
        public SiteStatisticsController(ISiteStatisticsRepository siteStatisticsRepository)
        {
            _siteStatisticsRepository = siteStatisticsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            SiteStatistics? siteStatistics = await _siteStatisticsRepository.Get();

            if(siteStatistics != null)
            {
                return Ok(siteStatistics);
            }

            return NotFound();
        }
    }
}
