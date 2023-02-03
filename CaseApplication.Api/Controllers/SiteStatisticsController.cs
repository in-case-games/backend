using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly ISiteStatisticsRepository _siteStatisticsRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public SiteStatisticsController(ISiteStatisticsRepository siteStatisticsRepository)
        {
            _siteStatisticsRepository = siteStatisticsRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            SiteStatistics? siteStatistics = await _siteStatisticsRepository.Get();

            if(siteStatistics != null)
            {
                siteStatistics.SiteBalance = 0;
                return Ok(siteStatistics);
            }

            return NotFound();
        }
    }
}
