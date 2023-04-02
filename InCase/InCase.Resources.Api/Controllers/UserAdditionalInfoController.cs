﻿using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            return info is null ?
                ResponseUtil.NotFound(nameof(UserAdditionalInfo)) :
                ResponseUtil.Ok(info);
        }

        [AllowAnonymous]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRole>? roles = await context.UserRoles
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(roles);
        }

        [AllowAnonymous]
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return ResponseUtil.Ok(role);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoDto infoDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? oldInfo = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(x => x.Id == infoDto.Id);

            if (oldInfo == null)
                return ResponseUtil.NotFound(nameof(UserAdditionalInfo));

            try
            {
                context.Entry(oldInfo).CurrentValues.SetValues(infoDto.Convert());
                await context.SaveChangesAsync();
            }
            catch(Exception ex) {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(infoDto);
        }
    }
}
