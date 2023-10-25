using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Entities;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/group-loot-box")]
    [ApiController]
    public class GroupLootBoxController : ControllerBase
    {
        private readonly IGroupLootBoxService _groupBoxService;

        public GroupLootBoxController(IGroupLootBoxService groupBoxService)
        {
            _groupBoxService = groupBoxService;
        }

        [ProducesResponseType(typeof(ApiResult<List<GroupLootBox>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<GroupLootBox> response = await _groupBoxService.GetAsync();

            return Ok(ApiResult<List<GroupLootBox>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            GroupLootBox response = await _groupBoxService.GetAsync(id);

            return Ok(ApiResult<GroupLootBox>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            GroupLootBox response = await _groupBoxService.GetAsync(name);

            return Ok(ApiResult<GroupLootBox>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(GroupLootBox request)
        {
            GroupLootBox response = await _groupBoxService.CreateAsync(request);

            return Ok(ApiResult<GroupLootBox>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(GroupLootBox request)
        {
            GroupLootBox response = await _groupBoxService.UpdateAsync(request);

            return Ok(ApiResult<GroupLootBox>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GroupLootBox>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            GroupLootBox response = await _groupBoxService.DeleteAsync(id);

            return Ok(ApiResult<GroupLootBox>.OK(response));
        }
    }
}
