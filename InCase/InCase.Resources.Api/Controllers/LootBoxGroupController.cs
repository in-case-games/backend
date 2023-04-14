using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBoxGroup> groups = await context.LootBoxGroups
                .Include(i => i.Group)
                .Include(i => i.Box)
                .AsNoTracking()
                .ToListAsync();

            return groups.Count == 0 ? 
                ResponseUtil.NotFound(nameof(LootBoxGroup)) : 
                ResponseUtil.Ok(groups);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxGroup? group = await context.LootBoxGroups
                .Include(i => i.Group)
                .Include(i => i.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return group is null ? 
                ResponseUtil.NotFound(nameof(LootBoxGroup)) : 
                ResponseUtil.Ok(group);
        }

        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(a => a.Id == id))
                return ResponseUtil.NotFound(nameof(Game));

            List<LootBoxGroup>? groups = await context.LootBoxGroups
                .Include(i => i.Group)
                .Include(i => i.Box)
                .AsNoTracking()
                .Where(w => w.GameId == id)
                .ToListAsync();

            return groups.Count == 0 ?
                ResponseUtil.NotFound(nameof(LootBoxGroup)) :
                ResponseUtil.Ok(groups);
        }

        [AllowAnonymous]
        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GroupLootBox> groups = await context.GroupLootBoxes
                .AsNoTracking()
                .ToListAsync();

            return groups.Count == 0 ?
                ResponseUtil.NotFound(nameof(GroupLootBox)) :
                ResponseUtil.Ok(groups);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxGroupDto groupDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(a => a.Id == groupDto.GameId))
                return ResponseUtil.NotFound(nameof(Game));
            if (!await context.GroupLootBoxes.AnyAsync(a => a.Id == groupDto.GroupId))
                return ResponseUtil.NotFound(nameof(Game));
            if (!await context.LootBoxes.AnyAsync(a => a.Id == groupDto.BoxId))
                return ResponseUtil.NotFound(nameof(Game));

            return await EndpointUtil.Create(groupDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroup(GroupLootBox group)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (await context.GroupLootBoxes.AnyAsync(a => a.Name == group.Name))
                return ResponseUtil.Conflict("The group name is already in use");

            group.Id = Guid.NewGuid();

            return await EndpointUtil.Create(group, context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<LootBoxGroup>(id, context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("group/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<GroupLootBox>(id, context);
        }
    }
}
