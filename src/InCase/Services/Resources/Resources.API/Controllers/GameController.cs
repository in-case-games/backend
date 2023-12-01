using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

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

        [ProducesResponseType(typeof(ApiResult<List<GameResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            List<GameResponse> response = await _gameService.GetAsync(cancellation);

            return Ok(ApiResult<List<GameResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            GameResponse response = await _gameService.GetAsync(id, cancellation);

            return Ok(ApiResult<GameResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name, CancellationToken cancellation)
        {
            GameResponse response = await _gameService.GetAsync(name, cancellation);

            return Ok(ApiResult<GameResponse>.OK(response));
        }
    }
}
