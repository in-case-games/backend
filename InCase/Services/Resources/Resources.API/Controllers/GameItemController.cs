using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Entities;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<GameItemResponse> response = await _itemService.GetAsync();

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            GameItemResponse response = await _itemService.GetAsync(id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            List<GameItemResponse> response = await _itemService.GetAsync(name);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("hash/{name}")]
        public async Task<IActionResult> GetByHashName(string hash)
        {
            List<GameItemResponse> response = await _itemService.GetByHashNameAsync(hash);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            List<GameItemQuality> response = await _itemService.GetQualitiesAsync();

            return Ok(ApiResult<List<GameItemQuality>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            List<GameItemRarity> response = await _itemService.GetRaritiesAsync();

            return Ok(ApiResult<List<GameItemRarity>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            List<GameItemType> response = await _itemService.GetTypesAsync();

            return Ok(ApiResult<List<GameItemType>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("quality/{id}")]
        public async Task<IActionResult> GetByQualityId(Guid id)
        {
            List<GameItemResponse> response = await _itemService.GetByQualityIdAsync(id);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("rarity/{id}")]
        public async Task<IActionResult> GetByRarityId(Guid id)
        {
            List<GameItemResponse> response = await _itemService.GetByRarityIdAsync(id);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("type/{id}")]
        public async Task<IActionResult> GetByTypeId(Guid id)
        {
            List<GameItemResponse> response = await _itemService.GetByTypeIdAsync(id);

            return Ok(ApiResult<List<GameItemResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Post(GameItemRequest request)
        {
            GameItemResponse response = await _itemService.CreateAsync(request);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Put(GameItemRequest request)
        {
            GameItemResponse response = await _itemService.UpdateAsync(request);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            GameItemResponse response = await _itemService.DeleteAsync(id);

            return Ok(ApiResult<GameItemResponse>.OK(response));
        }
    }
}
