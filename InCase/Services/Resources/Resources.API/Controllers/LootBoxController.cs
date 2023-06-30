﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Exceptions;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers
{
    [Route("api/loot-box")]
    [ApiController]
    public class LootBoxController : ControllerBase
    {
        private readonly ILootBoxService _lootBoxService;

        public LootBoxController(ILootBoxService lootBoxService)
        {
            _lootBoxService = lootBoxService;
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LootBoxResponse> response = await _lootBoxService.GetAsync();

            return Ok(ApiResult<List<LootBoxResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            List<LootBoxResponse> response = await _lootBoxService.GetByGameIdAsync(id);

            return Ok(ApiResult<List<LootBoxResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            LootBoxResponse response = await _lootBoxService.GetAsync(id);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            LootBoxResponse response = await _lootBoxService.GetAsync(name);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(LootBoxRequest request)
        {
            LootBoxResponse response = await _lootBoxService.CreateAsync(request);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(LootBoxRequest request)
        {
            LootBoxResponse response = await _lootBoxService.UpdateAsync(request);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LootBoxResponse response = await _lootBoxService.DeleteAsync(id);

            return Ok(ApiResult<LootBoxResponse>.OK(response));
        }
    }
}