using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class LootBoxInventoryService : ILootBoxInventoryService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxInventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LootBoxInventory?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.BoxInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation);

        public async Task CreateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
        {
            await _context.BoxInventories.AddAsync(new LootBoxInventory
            {
                Id = template.Id,
                BoxId = template.BoxId,
                ChanceWining = template.ChanceWining,
                ItemId = template.ItemId,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
        {
            var inventory = await _context.BoxInventories
                .FirstOrDefaultAsync(lbi => lbi.Id == template.Id, cancellation) ??
                throw new NotFoundException("Инвентарь не найден");

            _context.Entry(inventory).CurrentValues.SetValues(new LootBoxInventory
            {
                Id = template.Id,
                BoxId = template.BoxId,
                ChanceWining = template.ChanceWining,
                ItemId = template.ItemId,
            });
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var inventory = await _context.BoxInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation) ??
                throw new NotFoundException("Инвентарь не найден");

            _context.BoxInventories.Remove(inventory);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
