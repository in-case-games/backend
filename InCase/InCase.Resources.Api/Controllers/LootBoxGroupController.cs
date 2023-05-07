using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
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
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBoxGroup> groups = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(groups);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxGroup? group = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbg => lbg.Id == id);

            return group is null ? 
                ResponseUtil.NotFound("Кейс группа не найдена") : 
                ResponseUtil.Ok(group);
        }

        [AllowAnonymous]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetByGameId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (await context.Games.AnyAsync(g => g.Id == id) is false)
                return ResponseUtil.NotFound("Игра не найдена");

            List<LootBoxGroup> group = await context.LootBoxGroups
                .Include(lbg => lbg.Group)
                .Include(lbg => lbg.Box)
                .AsNoTracking()
                .Where(lbg => lbg.GameId == id)
                .ToListAsync();

            return ResponseUtil.Ok(group);
        }

        [AllowAnonymous]
        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GroupLootBox> groups = await context.GroupLootBoxes
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(groups);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxGroupDto groupDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(g => g.Id == groupDto.GameId))
                return ResponseUtil.NotFound("Игра не найдена");
            if (!await context.GroupLootBoxes.AnyAsync(glb => glb.Id == groupDto.GroupId))
                return ResponseUtil.NotFound("Группа кейсов не найдена");
            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == groupDto.BoxId))
                return ResponseUtil.NotFound("Кейс не найден");

            return await EndpointUtil.Create(groupDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroup(GroupLootBox group)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            group.Id = Guid.NewGuid();
            bool IsExistName = await context.GroupLootBoxes.AnyAsync(glb => glb.Name == group.Name);

            return IsExistName ? 
                ResponseUtil.Conflict("Имя группы уже используется") :
                await EndpointUtil.Create(group, context);
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
