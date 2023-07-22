using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/loot-box-opening")]
    [ApiController]
    public class LootBoxOpeningController : ControllerBase
    {
        private readonly ILootBoxOpeningService _openingService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public LootBoxOpeningController(ILootBoxOpeningService openingService)
        {
            _openingService = openingService;
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            GameItemResponse response = await _openingService.OpenBox(UserId, id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("virtual")]
        public async Task<IActionResult> GetVirtual(Guid id)
        {
            GameItemResponse response = await _openingService.OpenVirtualBox(UserId, id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }
    }
}
