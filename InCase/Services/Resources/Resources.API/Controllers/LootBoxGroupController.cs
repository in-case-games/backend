using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/loot-box-group")]
    [ApiController]
    public class LootBoxGroupController : ControllerBase
    {
        private readonly ILootBoxGroupService _boxGroupService;

        public LootBoxGroupController(ILootBoxGroupService boxGroupService)
        {
            _boxGroupService = boxGroupService;
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LootBoxGroupResponse> response = await _boxGroupService.GetAsync();

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxGroupResponse response = await _boxGroupService.GetAsync(id);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            List<LootBoxGroupResponse> response = await _boxGroupService.GetByGameIdAsync(id);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            List<LootBoxGroupResponse> response = await _boxGroupService.GetByBoxIdAsync(id);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetByGroupId(Guid id)
        {
            List<LootBoxGroupResponse> response = await _boxGroupService.GetByGroupIdAsync(id);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxGroupRequest request)
        {
            LootBoxGroupResponse response = await _boxGroupService.CreateAsync(request);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxGroupRequest request)
        {
            LootBoxGroupResponse response = await _boxGroupService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxGroupResponse response = await _boxGroupService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }
    }
}
