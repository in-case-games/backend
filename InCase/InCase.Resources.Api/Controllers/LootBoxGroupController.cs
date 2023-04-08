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
                .Include(i => i.Game)
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
                .Include(i => i.Group)
                .Include(i => i.Box)
                .Include(i => i.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return group is null ? 
                ResponseUtil.NotFound(nameof(LootBoxGroup)) : 
                ResponseUtil.Ok(group);
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

        [AuthorizeRoles(Roles.Owner, Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxGroupDto groupDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.LootBoxGroups.AddAsync(groupDto.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(groupDto.Convert());
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.Owner, Roles.Admin)]
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroup(GroupLootBox group)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            group.Id = Guid.NewGuid();

            try
            {
                await context.GroupLootBoxes.AddAsync(group);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(group);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.Owner, Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxGroup? group = await context.LootBoxGroups
                .FirstOrDefaultAsync(f => f.Id == id);

            if (group is null)
                return ResponseUtil.NotFound(nameof(LootBoxGroup));

            context.LootBoxGroups.Remove(group);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(nameof(LootBoxGroup));
        }

        [AuthorizeRoles(Roles.Owner, Roles.Admin)]
        [HttpDelete("group/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GroupLootBox? group = await context.GroupLootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (group is null)
                return ResponseUtil.NotFound(nameof(GroupLootBox));

            context.GroupLootBoxes.Remove(group);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(nameof(GroupLootBox));
        }
    }
}
