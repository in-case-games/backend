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
        private readonly ILootBoxGroupService _groupService;

        public LootBoxGroupController(ILootBoxGroupService groupService)
        {
            _groupService = groupService;
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _groupService.GetAsync(cancellation);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _groupService.GetAsync(id, cancellation);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id, CancellationToken cancellation)
        {
            var response = await _groupService.GetByGameIdAsync(id, cancellation);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
        {
            var response = await _groupService.GetByBoxIdAsync(id, cancellation);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetByGroupId(Guid id, CancellationToken cancellation)
        {
            var response = await _groupService.GetByGroupIdAsync(id, cancellation);

            return Ok(ApiResult<List<LootBoxGroupResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxGroupRequest request, CancellationToken cancellation)
        {
            var response = await _groupService.CreateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxGroupRequest request, CancellationToken cancellation)
        {
            var response = await _groupService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _groupService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<LootBoxGroupResponse>.OK(response));
        }
    }
}
