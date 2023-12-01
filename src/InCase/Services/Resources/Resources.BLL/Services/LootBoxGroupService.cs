﻿using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class LootBoxGroupService : ILootBoxGroupService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxGroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LootBoxGroupResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            LootBoxGroup group =  await _context.Groups
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
            List<LootBoxGroup> groups = await _context.Groups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return groups.ToResponse();
        }

        public async Task<List<LootBoxGroupResponse>> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default)
        {
            List<LootBoxGroup> groups = await _context.Groups
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
            List<LootBoxGroup> groups = await _context.Groups
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
            List<LootBoxGroup> groups = await _context.Groups
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
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ?? 
                throw new NotFoundException("Кейс не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GameId, cancellation) ??
                throw new NotFoundException("Игра не найдена");
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GroupId, cancellation) ??
                throw new NotFoundException("Группа не найдена");
            
            if (await _context.Groups
                .AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId, cancellation))
                throw new ConflictException("Кейс уже есть в этой группе");

            LootBoxGroup boxGroup = request.ToEntity(isNewGuid: true);

            await _context.Groups.AddAsync(boxGroup, cancellation);
            await _context.SaveChangesAsync(cancellation);

            boxGroup.Game = game;
            boxGroup.Group = group;
            boxGroup.Box = box;

            return boxGroup.ToResponse();
        }

        public async Task<LootBoxGroupResponse> UpdateAsync(LootBoxGroupRequest request, CancellationToken cancellation = default)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
                throw new NotFoundException("Кейс не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GameId, cancellation) ??
                throw new NotFoundException("Игра не найдена");
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GroupId, cancellation) ??
                throw new NotFoundException("Группа не найдена");

            if (await _context.Groups
                .AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId, cancellation))
                throw new ConflictException("Кейс уже есть в этой группе");

            LootBoxGroup boxGroup = request.ToEntity();

            _context.Groups.Update(boxGroup);
            await _context.SaveChangesAsync(cancellation);

            boxGroup.Game = game;
            boxGroup.Group = group;
            boxGroup.Box = box;

            return boxGroup.ToResponse();
        }

        public async Task<LootBoxGroupResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            LootBoxGroup group = await _context.Groups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbg => lbg.Id == id, cancellation) ??
                throw new NotFoundException("Группа кейсов не найдена");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellation);

            return group.ToResponse();
        }
    }
}
