﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResult<List<GameResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await gameService.GetAsync(cancellation);

            return Ok(ApiResult<List<GameResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await gameService.GetAsync(id, cancellation);

            return Ok(ApiResult<GameResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<GameResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name, CancellationToken cancellation)
        {
            var response = await gameService.GetAsync(name, cancellation);

            return Ok(ApiResult<GameResponse>.Ok(response));
        }
    }
}
