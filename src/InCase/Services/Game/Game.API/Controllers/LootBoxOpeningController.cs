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
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public LootBoxOpeningController(ILootBoxOpeningService openingService)
        {
            _openingService = openingService;
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _openingService.OpenBox(UserId, id, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/virtual")]
        public async Task<IActionResult> GetVirtual(Guid id, CancellationToken cancellation)
        {
            var response = await _openingService.OpenVirtualBox(UserId, id, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemBigOpenResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}&{count}/virtual")]
        public async Task<IActionResult> GetVirtual(Guid id, int count, CancellationToken cancellation)
        {
            var response = await _openingService.OpenVirtualBox(UserId, id, count, cancellation: cancellation);

            return Ok(ApiResult<List<GameItemBigOpenResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemBigOpenResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}&{count}/virtual/admin")]
        public async Task<IActionResult> GetVirtualByAdmin(Guid id, int count, CancellationToken cancellation)
        {
            var response = await _openingService.OpenVirtualBox(UserId, id, count, isAdmin: true, cancellation);

            return Ok(ApiResult<List<GameItemBigOpenResponse>>.OK(response));
        }
    }
}
