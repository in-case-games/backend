using Microsoft.EntityFrameworkCore;
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

        public async Task<LootBoxGroupResponse> GetAsync(Guid id)
        {
            LootBoxGroup group =  await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbg => lbg.Id == id) ??
                throw new NotFoundException("Группа кейсов не найдена");

            return group.ToResponse();
        }

        public async Task<List<LootBoxGroupResponse>> GetAsync()
        {
            List<LootBoxGroup> groups = await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .ToListAsync();

            return groups.ToResponse();
        }

        public async Task<List<LootBoxGroupResponse>> GetByBoxIdAsync(Guid id)
        {
            List<LootBoxGroup> groups = await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .Where(lbg => lbg.BoxId == id)
                .ToListAsync();

            return groups.ToResponse();
        }

        public async Task<List<LootBoxGroupResponse>> GetByGameIdAsync(Guid id)
        {
            List<LootBoxGroup> groups = await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .Where(lbg => lbg.GameId == id)
                .ToListAsync();

            return groups.ToResponse();
        }

        public async Task<List<LootBoxGroupResponse>> GetByGroupIdAsync(Guid id)
        {
            List<LootBoxGroup> groups = await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .Where(lbg => lbg.GroupId == id)
                .ToListAsync();

            return groups.ToResponse();
        }

        public async Task<LootBoxGroupResponse> CreateAsync(LootBoxGroupRequest request)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ?? 
                throw new NotFoundException("Кейс не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GameId) ??
                throw new NotFoundException("Игра не найдена");
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GroupId) ??
                throw new NotFoundException("Группа не найдена");
            
            if (await _context.BoxGroups
                .AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId))
                throw new ConflictException("Кейс уже есть в этой группе");

            LootBoxGroup boxGroup = request.ToEntity(isNewGuid: true);

            await _context.BoxGroups.AddAsync(boxGroup);
            await _context.SaveChangesAsync();

            boxGroup.Game = game;
            boxGroup.Group = group;
            boxGroup.Box = box;

            return boxGroup.ToResponse();
        }

        public async Task<LootBoxGroupResponse> UpdateAsync(LootBoxGroupRequest request)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GameId) ??
                throw new NotFoundException("Игра не найдена");
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.GroupId) ??
                throw new NotFoundException("Группа не найдена");

            if (await _context.BoxGroups
                .AnyAsync(lb => lb.BoxId == request.BoxId && lb.GroupId == request.GroupId))
                throw new ConflictException("Кейс уже есть в этой группе");

            LootBoxGroup boxGroup = request.ToEntity();

            _context.BoxGroups.Update(boxGroup);
            await _context.SaveChangesAsync();

            boxGroup.Game = game;
            boxGroup.Group = group;
            boxGroup.Box = box;

            return boxGroup.ToResponse();
        }

        public async Task<LootBoxGroupResponse> DeleteAsync(Guid id)
        {
            LootBoxGroup group = await _context.BoxGroups
                .Include(lbg => lbg.Game)
                .Include(lbg => lbg.Box)
                .Include(lbg => lbg.Group)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbg => lbg.Id == id) ??
                throw new NotFoundException("Группа кейсов не найдена");

            _context.BoxGroups.Remove(group);
            await _context.SaveChangesAsync();

            return group.ToResponse();
        }
    }
}
