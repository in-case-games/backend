using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Entities;
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

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<GameItemResponse> response = await _itemService.GetAsync();

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(GameItemResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            GameItemResponse response = await _itemService.GetAsync(id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            List<GameItemResponse> response = await _itemService.GetAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("hash/{name}")]
        public async Task<IActionResult> GetByHashName(string name)
        {
            List<GameItemResponse> response = await _itemService.GetByHashNameAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            List<GameItemResponse> response = await _itemService.GetByGameIdAsync(id);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemQuality>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            List<GameItemQuality> response = await _itemService.GetQualitiesAsync();

            return Ok(ApiResult<List<GameItemQuality>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemRarity>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            List<GameItemRarity> response = await _itemService.GetRaritiesAsync();

            return Ok(ApiResult<List<GameItemRarity>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemType>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            List<GameItemType> response = await _itemService.GetTypesAsync();

            return Ok(ApiResult<List<GameItemType>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("quality/{name}")]
        public async Task<IActionResult> GetByQuality(string name)
        {
            List<GameItemResponse> response = await _itemService.GetByQualityAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("rarity/{name}")]
        public async Task<IActionResult> GetByRarity(string name)
        {
            List<GameItemResponse> response = await _itemService.GetByRarityAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(List<GameItemResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("type/{name}")]
        public async Task<IActionResult> GetByType(string name)
        {
            List<GameItemResponse> response = await _itemService.GetByTypeAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(GameItemResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(GameItemRequest request)
        {
            GameItemResponse response = await _itemService.CreateAsync(request);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(GameItemResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(GameItemRequest request)
        {
            GameItemResponse response = await _itemService.UpdateAsync(request);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(GameItemResponse), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            GameItemResponse response = await _itemService.DeleteAsync(id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }
    }
}
