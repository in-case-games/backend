using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistics.API.Filters;
using Statistics.API.Common;
using Statistics.BLL.Services;
using Statistics.BLL.Models;

namespace Statistics.API.Controllers
{
    [Route("api/site-statistics")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly ISiteStatisticsService _siteStatisticsService;

        public SiteStatisticsController(ISiteStatisticsService siteStatisticsService)
        {
            _siteStatisticsService = siteStatisticsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            SiteStatisticsResponse response = await _siteStatisticsService.GetAsync();

            return Ok(ApiResult<SiteStatisticsResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner, Roles.Bot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            SiteStatisticsAdminResponse response = await _siteStatisticsService.GetAdminAsync();

            return Ok(ApiResult<SiteStatisticsAdminResponse>.OK(response));
        }
    }
}
