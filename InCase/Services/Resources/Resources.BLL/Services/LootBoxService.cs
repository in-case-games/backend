using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class LootBoxService : ILootBoxService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LootBoxResponse> GetAsync(Guid id)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> GetAsync(string name)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Name == name) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetAsync()
        {
            List<LootBox> boxes = await _context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id)
        {
            List<LootBox> boxes = await _context.LootBoxes
                .AsNoTracking()
                .Where(lb => lb.GameId == id)
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<LootBoxResponse> CreateAsync(LootBoxRequest request)
        {
            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId))
                throw new NotFoundException("Игра не найдена");

            bool IsNameBusy = await _context.LootBoxes
                .AnyAsync(lb => lb.Name == request.Name || lb.HashName == request.HashName);

            if (IsNameBusy) throw new ConflictException("Имя или хэш уже занят");

            LootBox box = request.ToEntity(isNewGuid: true);

            await _context.LootBoxes.AddAsync(box);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> UpdateAsync(LootBoxRequest request)
        {
            if (!await _context.LootBoxes.AnyAsync(lb => lb.Id == request.Id))
                throw new NotFoundException("Кейс не найден");
            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId))
                throw new NotFoundException("Игра не найдена");

            LootBox newBox = request.ToEntity(isNewGuid: false);

            _context.LootBoxes.Update(newBox);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            return newBox.ToResponse();
        }

        public async Task<LootBoxResponse> DeleteAsync(Guid id)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            _context.LootBoxes.Remove(box);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            return box.ToResponse();
        }
    }
}
