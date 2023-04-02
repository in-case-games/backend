using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/game-item")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public GameItemController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameItem> gameItems = await context.GameItems
                .AsNoTracking()
                .Include(x => x.Type)
                .ToListAsync();

            return ResponseUtil.Ok(gameItems);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? gameItem = await context.GameItems
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Rarity)
                .Include(x => x.Quality)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItem is null)
                return ResponseUtil.NotFound(nameof(GameItem));

            return ResponseUtil.Ok(gameItem);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            return await EndpointUtil.GetAll<GameItemQuality>(_contextFactory);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<GameItemType>(_contextFactory);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            return await EndpointUtil.GetAll<GameItemRarity>(_contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(GameItemDto gameItem)
        {
            return await EndpointUtil.Create(gameItem.Convert(), _contextFactory);  
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("qualities")]
        public async Task<IActionResult> CreateQuality(GameItemQuality quality)
        {
            return await EndpointUtil.Create(quality, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("rarities")]
        public async Task<IActionResult> CreateRarity(GameItemRarity rarity)
        {
            return await EndpointUtil.Create(rarity, _contextFactory);     
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("types")]
        public async Task<IActionResult> CreateRarity(GameItemType type)
        {
            return await EndpointUtil.Create(type, _contextFactory);          
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto item)
        {
            return await EndpointUtil.Update(item.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<GameItem>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("qualities/{id}")]
        public async Task<IActionResult> DeleteItemQuality(Guid id)
        {
            return await EndpointUtil.Delete<GameItemQuality>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("rarities/{id}")]
        public async Task<IActionResult> DeleteItemRarity(Guid id)
        {
            return await EndpointUtil.Delete<GameItemRarity>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteItemType(Guid id)
        {
            return await EndpointUtil.Delete<GameItemType>(id, _contextFactory);
        }
    }
}
