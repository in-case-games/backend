using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class LootBoxInventoryService : ILootBoxInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxInventoryService(ApplicationDbContext context,BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<LootBoxInventoryResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation) ?? 
                throw new NotFoundException("Содержимое кейса не найдено");

            return inventory.ToResponse();
        }

        public async Task<List<LootBoxInventoryResponse>> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default)
        {
            if (!await _context.LootBoxes.AnyAsync(lbi => lbi.Id == id, cancellation))
                throw new NotFoundException("Кейс не найден");

            var inventories = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .AsNoTracking()
                .Where(lbi => lbi.BoxId == id)
                .ToListAsync(cancellation);

            return inventories.ToResponse();
        }

        public async Task<List<LootBoxInventoryResponse>> GetByItemIdAsync(Guid id, CancellationToken cancellation = default)
        {
            if (!await _context.Items.AnyAsync(lbi => lbi.Id == id, cancellation))
                throw new NotFoundException("Предмет не найден");

            var inventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .Where(lbi => lbi.ItemId == id)
                .ToListAsync(cancellation);

            return inventories.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> CreateAsync(LootBoxInventoryRequest request, CancellationToken cancellation = default)
        {
            request.Id = Guid.NewGuid();

            var box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            var item = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == request.ItemId, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            var inventory = new LootBoxInventory()
            {
                Id = request.Id,
                BoxId = request.BoxId,
                ItemId = request.ItemId,
                ChanceWining = request.ChanceWining
            };

            if (box.GameId != item.GameId) 
                throw new ConflictException("Кейс и предмет должны быть с одной игры");

            await _context.BoxInventories.AddAsync(inventory, cancellation);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(new LootBoxInventoryTemplate()
            {
                Id = request.Id,
                BoxId = request.BoxId,
                ChanceWining = request.ChanceWining,
                ItemId = request.ItemId
            }, cancellation);

            inventory.Item = item;
            inventory.Box = box;

            return inventory.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> UpdateAsync(LootBoxInventoryRequest request, CancellationToken cancellation = default)
        {
            var box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            var item = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == request.ItemId, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            if (!await _context.BoxInventories.AnyAsync(lbi => lbi.Id == request.Id, cancellation))
                throw new NotFoundException("Содержимое кейса не найдено");
            if (box.GameId != item.GameId)
                throw new ConflictException("Кейс и предмет должны быть с одной игры");

            var inventory = new LootBoxInventory()
            {
                Id = request.Id,
                BoxId = request.BoxId,
                ItemId = request.ItemId,
                ChanceWining = request.ChanceWining,
                Item = item,
                Box = box
            };

            _context.BoxInventories.Update(inventory);
            await _publisher.SendAsync(new LootBoxInventoryTemplate()
            {
                Id = request.Id,
                BoxId = request.BoxId,
                ChanceWining = request.ChanceWining,
                ItemId = request.ItemId
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return inventory.ToResponse();
        }

        public async Task<LootBoxInventoryResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi!.Item!.Rarity)
                .Include(lbi => lbi!.Item!.Quality)
                .Include(lbi => lbi!.Item!.Type)
                .Include(lbi => lbi!.Item!.Game)
                .Include(lbi => lbi.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation) ?? 
                throw new NotFoundException("Содержимое кейса не найдено");

            _context.BoxInventories.Remove(inventory);
            await _publisher.SendAsync(new LootBoxInventoryTemplate()
            {
                Id = inventory.Id,
                BoxId = inventory.BoxId,
                ChanceWining = inventory.ChanceWining,
                ItemId = inventory.ItemId,
                IsDeleted = true
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return inventory.ToResponse();
        }
    }
}
