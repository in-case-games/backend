using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.DAL.Entities;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/group-loot-box")]
    [ApiController]
    public class GroupLootBoxController(IGroupLootBoxService groupBoxService) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResult<List<GroupLootBox>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await groupBoxService.GetAsync(cancellation);

            return Ok(ApiResult<List<GroupLootBox>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await groupBoxService.GetAsync(id, cancellation);

            return Ok(ApiResult<GroupLootBox>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name, CancellationToken cancellation)
        {
            var response = await groupBoxService.GetAsync(name, cancellation);

            return Ok(ApiResult<GroupLootBox>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(GroupLootBox request, CancellationToken cancellation)
        {
            var response = await groupBoxService.CreateAsync(request, cancellation);

            return Ok(ApiResult<GroupLootBox>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(GroupLootBox request, CancellationToken cancellation)
        {
            var response = await groupBoxService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<GroupLootBox>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await groupBoxService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<GroupLootBox>.Ok(response));
        }
    }
}
