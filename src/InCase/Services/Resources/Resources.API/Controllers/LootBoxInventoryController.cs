using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/loot-box-inventory")]
    [ApiController]
    public class LootBoxInventoryController : ControllerBase
    {
        private readonly ILootBoxInventoryService _inventoryService;

        public LootBoxInventoryController(ILootBoxInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxInventoryResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            LootBoxInventoryResponse response = await _inventoryService.GetAsync(id, cancellation);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
        {
            List<LootBoxInventoryResponse> response = await _inventoryService
                .GetByBoxIdAsync(id, cancellation);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id, CancellationToken cancellation)
        {
            List<LootBoxInventoryResponse> response = await _inventoryService
                .GetByItemIdAsync(id, cancellation);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxInventoryResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxInventoryRequest request, CancellationToken cancellation)
        {
            LootBoxInventoryResponse response = await _inventoryService.CreateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxInventoryResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxInventoryRequest request, CancellationToken cancellation)
        {
            LootBoxInventoryResponse response = await _inventoryService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxInventoryResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            LootBoxInventoryResponse response = await _inventoryService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }
    }
}
