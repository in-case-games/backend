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
    [Route("api/game-item")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly IGameItemService _itemService;

        public GameItemController(IGameItemService itemService)
        {
            _itemService = itemService;
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _itemService.GetAsync(cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _itemService.GetAsync(id, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name, CancellationToken cancellation)
        {
            var response = await _itemService.GetAsync(name, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("hash/{name}")]
        public async Task<IActionResult> GetByHashName(string name, CancellationToken cancellation)
        {
            var response = await _itemService.GetByHashNameAsync(name, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id, CancellationToken cancellation)
        {
            var response = await _itemService.GetByGameIdAsync(id, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemQuality>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities(CancellationToken cancellation)
        {
            var response = await _itemService.GetQualitiesAsync(cancellation);

            return Ok(ApiResult<List<GameItemQuality>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemRarity>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities(CancellationToken cancellation)
        {
            var response = await _itemService.GetRaritiesAsync(cancellation);

            return Ok(ApiResult<List<GameItemRarity>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemType>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes(CancellationToken cancellation)
        {
            var response = await _itemService.GetTypesAsync(cancellation);

            return Ok(ApiResult<List<GameItemType>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("quality/{name}")]
        public async Task<IActionResult> GetByQuality(string name, CancellationToken cancellation)
        {
            var response = await _itemService.GetByQualityAsync(name, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("rarity/{name}")]
        public async Task<IActionResult> GetByRarity(string name, CancellationToken cancellation)
        {
            var response = await _itemService.GetByRarityAsync(name, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<GameItemResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("type/{name}")]
        public async Task<IActionResult> GetByType(string name, CancellationToken cancellation)
        {
            var response = await _itemService.GetByTypeAsync(name, cancellation);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [RequestSizeLimit(8388608)]
        [HttpPost]
        public async Task<IActionResult> Post(GameItemRequest request, CancellationToken cancellation)
        {
            var response = await _itemService.CreateAsync(request, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [RequestSizeLimit(8388608)]
        [HttpPut]
        public async Task<IActionResult> Put(GameItemRequest request, CancellationToken cancellation)
        {
            var response = await _itemService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _itemService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }
    }
}
