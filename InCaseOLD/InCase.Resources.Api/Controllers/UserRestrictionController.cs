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
    [Route("api/user-restriction")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserRestriction restriction = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Эффект не найден");

            return ResponseUtil.Ok(restriction);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == UserId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (!await context.Users.AnyAsync(u => u.Id == id, cancellationToken))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByIds(Guid ownerId, Guid userId, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellationToken))
                throw new NotFoundCodeException("Пользователь не найден");
            if (!await context.Users.AnyAsync(u => u.Id == ownerId, cancellationToken))
                throw new NotFoundCodeException("Обвинитель не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == ownerId && ur.UserId == userId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == UserId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner/{userId}")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellationToken))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == UserId && ur.UserId == userId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<RestrictionType>(_context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            restrictionDto.OwnerId = UserId;

            RestrictionType type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == restrictionDto.TypeId, cancellationToken) ??
                throw new NotFoundCodeException("Тип эффекта не найден");

            User user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == restrictionDto.UserId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                throw new ForbiddenCodeException("Эффект можно наложить только на пользователя");

            restrictionDto = await CheckUserRestriction(restrictionDto, type, context, cancellationToken);

            return await EndpointUtil.Create(restrictionDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestrictionDto restrictionDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            restrictionDto.OwnerId = UserId;

            RestrictionType type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == restrictionDto.TypeId, cancellationToken) ??
                throw new NotFoundCodeException("Тип эффекта не найден");

            User user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == restrictionDto.UserId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");

            if (!await context.UserRestrictions.AnyAsync(a => a.Id == restrictionDto.Id, cancellationToken))
                throw new NotFoundCodeException("Эффект не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                throw new ForbiddenCodeException("Эффект можно наложить только на пользователя");

            restrictionDto = await CheckUserRestriction(restrictionDto, type, context, cancellationToken);

            return await EndpointUtil.Update(restrictionDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserRestriction restriction = await context.UserRestrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Эффект не найден");

            return await EndpointUtil.Delete(restriction, context, cancellationToken);
        }

        private static async Task<UserRestrictionDto> CheckUserRestriction(UserRestrictionDto restrictionDto,
            RestrictionType type,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == restrictionDto.UserId)
                .ToListAsync(cancellationToken);

            int numberWarns = (type.Name == "warn") ? 1 : 0;

            foreach (var restriction in restrictions)
            {
                if (restriction.Type!.Name == "warn" && restriction.Id != restrictionDto.Id)
                    ++numberWarns;
            }

            if (numberWarns >= 3)
            {
                RestrictionType? ban = await context.RestrictionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Name == "ban", cancellationToken);

                restrictionDto.TypeId = ban!.Id;
                restrictionDto.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return restrictionDto;
        }
    }
}