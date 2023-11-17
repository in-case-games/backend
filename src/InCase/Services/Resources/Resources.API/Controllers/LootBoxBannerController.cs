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

        [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LootBoxBannerResponse> response = await _bannerService.GetAsync();

            return Ok(ApiResult<List<LootBoxBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("active/{isActive}")]
        public async Task<IActionResult> GetByIsActive(bool isActive = true)
        {
            List<LootBoxBannerResponse> response = await _bannerService.GetAsync(isActive);

            return Ok(ApiResult<List<LootBoxBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxBannerResponse response = await _bannerService.GetAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            LootBoxBannerResponse response = await _bannerService.GetByBoxIdAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), 
            (int)HttpStatusCode.OK)]
        [RequestSizeLimit(8388608)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxBannerRequest request)
        {
            LootBoxBannerResponse response = await _bannerService.CreateAsync(request);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>),
            (int)HttpStatusCode.OK)]
        [RequestSizeLimit(8388608)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxBannerRequest request)
        {
            LootBoxBannerResponse response = await _bannerService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxBannerResponse response = await _bannerService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }
    }
}
