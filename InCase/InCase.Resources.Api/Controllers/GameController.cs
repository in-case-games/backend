using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GameController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<Game> games = await context.Games
                .Include(i => i.Boxes)
                .Include(i => i.Items)
                .AsNoTracking()
                .ToListAsync();

            return games.Count == 0 ?
                ResponseUtil.NotFound(nameof(Game)) : 
                ResponseUtil.Ok(games);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Game? game = await context.Games
                .Include(i => i.Boxes)
                .Include(i => i.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return game is null ? 
                ResponseUtil.NotFound(nameof(Game)) : 
                ResponseUtil.Ok(game);
        }
    }
}
