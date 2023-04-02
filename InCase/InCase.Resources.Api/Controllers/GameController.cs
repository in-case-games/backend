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
            return await EndpointUtil.GetAll<Game>(_contextFactory);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await EndpointUtil.GetById<Game>(id, _contextFactory);
        }
        [AllowAnonymous]
        [HttpGet("platforms")]
        public async Task<IActionResult> GetPlatforms()
        {
            return await EndpointUtil.GetAll<GamePlatform>(_contextFactory);
        }
        [AllowAnonymous]
        [HttpGet("platforms/{id}")]
        public async Task<IActionResult> GetPlatform(Guid id)
        {
            return await EndpointUtil.GetById<GamePlatform>(id, _contextFactory);
        }
    }
}
