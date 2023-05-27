using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Entities;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;

namespace Resources.API.Controllers
{
    [Route("api/loot-box-banner")]
    [ApiController]
    public class LootBoxBannerController : ControllerBase
    {
        private readonly ILootBoxBannerService _boxBannerService;

        public LootBoxBannerController(ILootBoxBannerService boxBannerService)
        {
            _boxBannerService = boxBannerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LootBoxBannerResponse> response = await _boxBannerService.GetAsync();

            return Ok(ApiResult<List<LootBoxBannerResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxBannerResponse response = await _boxBannerService.GetAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            LootBoxBannerResponse response = await _boxBannerService.GetByBoxIdAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxBannerRequest request)
        {
            LootBoxBannerResponse response = await _boxBannerService.CreateAsync(request);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxBannerRequest request)
        {
            LootBoxBannerResponse response = await _boxBannerService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxBannerResponse response = await _boxBannerService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxBannerResponse>.OK(response));
        }
    }
}
