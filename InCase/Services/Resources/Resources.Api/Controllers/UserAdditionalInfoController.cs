﻿using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserAdditionalInfo info = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Дополнительная информация не найдена");

            return ResponseUtil.Ok(info.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserAdditionalInfo info = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");

            return ResponseUtil.Ok(info.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<UserRole>(_context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("guest-mode")]
        public async Task<IActionResult> OnOffGuestMode(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserAdditionalInfo info = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");

            info.IsGuestMode = !info.IsGuestMode;

            context.UserAdditionalInfos.Attach(info);
            context.Entry(info).Property(p => p.IsGuestMode).IsModified = true;

            await context.SaveChangesAsync(cancellationToken);

            return info.IsGuestMode ? 
                ResponseUtil.Ok("Гостевой мод включен") : 
                ResponseUtil.Ok("Гостевой мод выключен");
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoDto infoDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (!await context.UserRoles.AnyAsync(ur => ur.Id == infoDto.RoleId, cancellationToken))
                throw new NotFoundCodeException("Роль не найдена");
            if (!await context.Users.AnyAsync(u => u.Id == infoDto.UserId, cancellationToken))
                throw new NotFoundCodeException("Пользователь не найден");

            return await EndpointUtil.Update(infoDto.Convert(false), context, cancellationToken);
        }
    }
}