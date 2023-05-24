using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;

namespace Resources.API.Controllers
{
    [Route("api/loot-box-inventory")]
    [ApiController]
    public class LootBoxInventoryController : ControllerBase
    {
        private readonly ILootBoxInventoryService _boxInventoryService;

        public LootBoxInventoryController(ILootBoxInventoryService boxInventoryService)
        {
            _boxInventoryService = boxInventoryService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.GetAsync(id);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            List<LootBoxInventoryResponse> response = await _boxInventoryService
                .GetByBoxIdAsync(id);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<LootBoxInventoryResponse> response = await _boxInventoryService
                .GetByItemIdAsync(id);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxInventoryRequest request)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.CreateAsync(request);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxInventoryRequest request)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }
    }
}
