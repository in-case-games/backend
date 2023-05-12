using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/loot-box-group")]
    [ApiController]
    public class LootBoxGroupController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public LootBoxGroupController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<LootBoxGroup> groups = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(groups);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            LootBoxGroup group = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbg => lbg.Id == id, cancellationToken) ?? 
                throw new NotFoundCodeException("Кейс группа не найдена");

            return ResponseUtil.Ok(group);
        }

        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (!await context.Games.AnyAsync(g => g.Id == id, cancellationToken))
                throw new NotFoundCodeException("Игра не найдена");

            List<LootBoxGroup> group = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .Where(lbg => lbg.GameId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(group);
        }

        [AllowAnonymous]
        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<GroupLootBox> groups = await context.GroupLootBoxes
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(groups);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxGroupDto groupDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (!await context.Games.AnyAsync(g => g.Id == groupDto.GameId, cancellationToken))
                throw new NotFoundCodeException("Игра не найдена");
            if (!await context.GroupLootBoxes.AnyAsync(glb => glb.Id == groupDto.GroupId, cancellationToken))
                throw new NotFoundCodeException("Группа кейсов не найдена");
            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == groupDto.BoxId, cancellationToken))
                throw new NotFoundCodeException("Кейс не найден");

            return await EndpointUtil.Create(groupDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroup(GroupLootBox group, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            group.Id = Guid.NewGuid();
            bool isExistName = await context.GroupLootBoxes.AnyAsync(glb => glb.Name == group.Name, cancellationToken);

            return isExistName ?
                throw new ConflictCodeException("Имя группы уже используется") :
                await EndpointUtil.Create(group, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<LootBoxGroup>(id, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("group/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<GroupLootBox>(id, context, cancellationToken);
        }
    }
}
