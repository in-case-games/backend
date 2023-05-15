using InCase.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistics.BLL.Services;
using Statistics.DAL.Entities;
using Statistics.DAL.Helpers;

namespace Statistics.API.Controllers
{
    [Route("api/statistics/site")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly SiteStatisticsService _siteStatiticsService;

        public SiteStatisticsController(SiteStatisticsService siteStatisticsService)
        {
            _siteStatiticsService = siteStatisticsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            SiteStatistics statistics = await _siteStatiticsService.Get();

            return ApiResult.OK(statistics.ToResponse());
        }

        [AllowAnonymous]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            SiteStatisticsAdmin statisticsAdmin = await _siteStatiticsService.GetAdmin();

            return ApiResult.OK(statisticsAdmin.ToResponse());
        }
    }
}
