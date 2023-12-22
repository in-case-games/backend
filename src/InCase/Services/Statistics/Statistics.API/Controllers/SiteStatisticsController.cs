using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistics.API.Filters;
using Statistics.API.Common;
using Statistics.BLL.Services;
using Statistics.BLL.Models;
using System.Net;

namespace Statistics.API.Controllers
{
    [Route("api/site-statistics")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly ISiteStatisticsService _statisticsService;

        public SiteStatisticsController(ISiteStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [ProducesResponseType(typeof(ApiResult<SiteStatisticsResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _statisticsService.GetAsync(cancellation);

            return Ok(ApiResult<SiteStatisticsResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SiteStatisticsAdminResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner, Roles.Bot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin(CancellationToken cancellation)
        {
            var response = await _statisticsService.GetAdminAsync(cancellation);

            return Ok(ApiResult<SiteStatisticsAdminResponse>.Ok(response));
        }
    }
}
