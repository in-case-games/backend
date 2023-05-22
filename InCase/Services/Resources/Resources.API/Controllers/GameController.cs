using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;

namespace Resources.API.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<GameResponse> response = await _gameService.GetAsync();

            return Ok(ApiResult<List<GameResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            GameResponse response = await _gameService.GetAsync(id);

            return Ok(ApiResult<GameResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            GameResponse response = await _gameService.GetAsync(name);

            return Ok(ApiResult<GameResponse>.OK(response));
        }
    }
}
