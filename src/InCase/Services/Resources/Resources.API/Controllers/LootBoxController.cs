using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/loot-box")]
    [ApiController]
    public class LootBoxController : ControllerBase
    {
        private readonly ILootBoxService _boxService;

        public LootBoxController(ILootBoxService boxService)
        {
            _boxService = boxService;
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LootBoxResponse> response = await _boxService.GetAsync();

            return Ok(ApiResult<List<LootBoxResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            List<LootBoxResponse> response = await _boxService.GetByGameIdAsync(id);

            return Ok(ApiResult<List<LootBoxResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxResponse response = await _boxService.GetAsync(id);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            LootBoxResponse response = await _boxService.GetAsync(name);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [RequestSizeLimit(8388608)]
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile image, [FromForm] LootBoxRequest request)
        {
            LootBoxResponse response = await _boxService.CreateAsync(request, image);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [RequestSizeLimit(8388608)]
        [HttpPut]
        public async Task<IActionResult> Put(IFormFile image, [FromForm] LootBoxRequest request)
        {
            LootBoxResponse response = await _boxService.UpdateAsync(request, image);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxResponse response = await _boxService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }
    }
}
