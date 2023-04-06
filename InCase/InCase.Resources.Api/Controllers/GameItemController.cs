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
                .Include(i => i.Type)
                .Include(i => i.Quality)
                .Include(i => i.Rarity)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(gameItems);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? gameItem = await context.GameItems
                .Include(i => i.Type)
                .Include(i => i.Rarity)
                .Include(i => i.Quality)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return gameItem is null ? 
                ResponseUtil.NotFound(nameof(GameItem)) : 
                ResponseUtil.Ok(gameItem);
        }

        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            return await EndpointUtil.GetAll<GameItemQuality>(_contextFactory);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<GameItemType>(_contextFactory);
        }

        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            return await EndpointUtil.GetAll<GameItemRarity>(_contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(GameItemDto gameItem)
        {
            return await EndpointUtil.Create(gameItem.Convert(), _contextFactory);  
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto item)
        {
            return await EndpointUtil.Update(item.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<GameItem>(id, _contextFactory);
        }
    }
}
