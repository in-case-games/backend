using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services;
public class LootBoxGroupService(ApplicationDbContext context) : ILootBoxGroupService
{
    public async Task<LootBoxGroupResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var group =  await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .FirstOrDefaultAsync(lbg => lbg.Id == id, cancellation) ??
            throw new NotFoundException("Группа кейсов не найдена");

        return group.ToResponse();
    }

    public async Task<List<LootBoxGroupResponse>> GetAsync(CancellationToken cancellation = default)
    {
        var groups = await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .ToListAsync(cancellation);

        return groups.ToResponse();
    }

    public async Task<List<LootBoxGroupResponse>> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var groups = await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .Where(lbg => lbg.BoxId == id)
            .ToListAsync(cancellation);

        return groups.ToResponse();
    }

    public async Task<List<LootBoxGroupResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var groups = await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .Where(lbg => lbg.GameId == id)
            .ToListAsync(cancellation);

        return groups.ToResponse();
    }

    public async Task<List<LootBoxGroupResponse>> GetByGroupIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var groups = await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .Where(lbg => lbg.GroupId == id)
            .ToListAsync(cancellation);

        return groups.ToResponse();
    }

    public async Task<LootBoxGroupResponse> CreateAsync(LootBoxGroupRequest request, CancellationToken cancellation = default)
    {
        var box = await context.LootBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ?? 
            throw new NotFoundException("Кейс не найден");
        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.GameId, cancellation) ??
            throw new NotFoundException("Игра не найдена");
        var group = await context.GroupLootBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.GroupId, cancellation) ??
            throw new NotFoundException("Группа не найдена");
        
        if (await context.LootBoxGroups.AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId, cancellation))
            throw new ConflictException("Кейс уже есть в этой группе");

        var boxGroup = new LootBoxGroup
        {
            Id = Guid.NewGuid(),
            BoxId = request.BoxId,
            GameId = request.GameId,
            GroupId = request.GroupId,
        };

        await context.LootBoxGroups.AddAsync(boxGroup, cancellation);
        await context.SaveChangesAsync(cancellation);

        boxGroup.Game = game;
        boxGroup.Group = group;
        boxGroup.Box = box;

        return boxGroup.ToResponse();
    }

    public async Task<LootBoxGroupResponse> UpdateAsync(LootBoxGroupRequest request, CancellationToken cancellation = default)
    {
        var box = await context.LootBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
            throw new NotFoundException("Кейс не найден");
        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.GameId, cancellation) ??
            throw new NotFoundException("Игра не найдена");
        var group = await context.GroupLootBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.GroupId, cancellation) ??
            throw new NotFoundException("Группа не найдена");

        if (await context.LootBoxGroups.AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId, cancellation))
            throw new ConflictException("Кейс уже есть в этой группе");

        var boxGroup = new LootBoxGroup
        {
            Id = request.Id,
            BoxId = request.BoxId,
            GameId = request.GameId,
            GroupId = request.GroupId
        };

        context.LootBoxGroups.Update(boxGroup);
        await context.SaveChangesAsync(cancellation);

        boxGroup.Game = game;
        boxGroup.Group = group;
        boxGroup.Box = box;

        return boxGroup.ToResponse();
    }

    public async Task<LootBoxGroupResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var group = await context.LootBoxGroups
            .Include(lbg => lbg.Game)
            .Include(lbg => lbg.Box)
            .Include(lbg => lbg.Group)
            .AsNoTracking()
            .FirstOrDefaultAsync(lbg => lbg.Id == id, cancellation) ??
            throw new NotFoundException("Группа кейсов не найдена");

        context.LootBoxGroups.Remove(group);
        await context.SaveChangesAsync(cancellation);

        return group.ToResponse();
    }
}