using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ILootBoxInventoryService _boxInventoryService;

        public LootBoxInventoryController(ILootBoxInventoryService boxInventoryService)
        {
            _boxInventoryService = boxInventoryService;
        }

        [ProducesResponseType(typeof(LootBoxInventoryResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.GetAsync(id);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(List<LootBoxInventoryResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            List<LootBoxInventoryResponse> response = await _boxInventoryService
                .GetByBoxIdAsync(id);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<LootBoxInventoryResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<LootBoxInventoryResponse> response = await _boxInventoryService
                .GetByItemIdAsync(id);

            return Ok(ApiResult<List<LootBoxInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(LootBoxInventoryResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxInventoryRequest request)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.CreateAsync(request);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(LootBoxInventoryResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxInventoryRequest request)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(LootBoxInventoryResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxInventoryResponse response = await _boxInventoryService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxInventoryResponse>.OK(response));
        }
    }
}
