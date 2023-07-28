﻿using Identity.API.Common;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.API.Controllers
{
    [Route("api/user-role")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _roleService;

        public UserRoleController(IUserRoleService roleService)
        {
            _roleService = roleService;
        }

        [ProducesResponseType(typeof(ApiResult<UserRoleResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserRoleResponse response = await _roleService.GetAsync(id);

            return Ok(ApiResult<UserRoleResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRoleResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserRoleResponse> response = await _roleService.GetAsync();

            return Ok(ApiResult<List<UserRoleResponse>>.OK(response));
        }
    }
}