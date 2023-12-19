using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Entities;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/loot-box-banner")]
    [ApiController]
    public class LootBoxBannerController : ControllerBase
    {
        private readonly ILootBoxBannerService _bannerService;

        public LootBoxBannerController(ILootBoxBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _bannerService.GetAsync(cancellation);

            return Ok(ApiResult<List<LootBoxBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("active/{isActive}")]
        public async Task<IActionResult> GetByIsActive(CancellationToken cancellation, bool isActive = true)
        {
            var response = await _bannerService.GetAsync(isActive, cancellation);

            return Ok(ApiResult<List<LootBoxBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _bannerService.GetAsync(id, cancellation);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
        {
            var response = await _bannerService.GetByBoxIdAsync(id, cancellation);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
        [RequestSizeLimit(8388608)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxBannerRequest request, CancellationToken cancellation)
        {
            var response = await _bannerService.CreateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
        [RequestSizeLimit(8388608)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxBannerRequest request, CancellationToken cancellation)
        {
            var response = await _bannerService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _bannerService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }
    }
}
