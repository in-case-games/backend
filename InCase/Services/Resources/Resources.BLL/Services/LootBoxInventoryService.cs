using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class LootBoxInventoryService : ILootBoxInventoryService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxInventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LootBoxInventoryResponse> GetAsync(Guid id)
        {
            LootBoxInventory inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id) ?? 
                throw new NotFoundException("Содержимое кейса не найдено");

            return inventory.ToResponse();
        }

        public async Task<List<LootBoxInventoryResponse>> GetByBoxIdAsync(Guid id)
        {
            if (!await _context.LootBoxes.AnyAsync(lbi => lbi.Id == id))
                throw new NotFoundException("Кейс не найден");

            List<LootBoxInventory> inventories = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .AsNoTracking()
                .Where(lbi => lbi.BoxId == id)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<List<LootBoxInventoryResponse>> GetByItemIdAsync(Guid id)
        {
            if (!await _context.GameItems.AnyAsync(lbi => lbi.Id == id))
                throw new NotFoundException("Предмет не найден");

            List<LootBoxInventory> inventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .Where(lbi => lbi.ItemId == id)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> CreateAsync(LootBoxInventoryRequest request)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            GameItem item = await _context.GameItems
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == request.ItemId) ??
                throw new NotFoundException("Предмет не найден");

            if (box.GameId != item.GameId) 
                throw new ConflictException("Кейс и предмет должны быть с одной игры");

            LootBoxInventory inventory = request.ToEntity(true);

            await _context.BoxInventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            inventory.Item = item;
            inventory.Box = box;

            return inventory.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> UpdateAsync(LootBoxInventoryRequest request)
        {
            if (!await _context.BoxInventories.AnyAsync(lbi => lbi.Id == request.Id))
                throw new NotFoundException("Содержимое кейса не найдено");

            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            GameItem item = await _context.GameItems
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == request.ItemId) ??
                throw new NotFoundException("Предмет не найден");

            if (box.GameId != item.GameId)
                throw new ConflictException("Кейс и предмет должны быть с одной игры");

            LootBoxInventory inventory = request.ToEntity();

            _context.BoxInventories.Update(inventory);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            inventory.Item = item;
            inventory.Box = box;

            return inventory.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> DeleteAsync(Guid id)
        {
            LootBoxInventory inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id)
                ?? throw new NotFoundException("Содержимое кейса не найдено");

            _context.BoxInventories.Remove(inventory);
            await _context.SaveChangesAsync();

            //Notify rabbit mq

            return inventory.ToResponse();
        }
    }
}
