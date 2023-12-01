using Game.BLL.Exceptions;
using Game.BLL.Helpers;
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

        public async Task<LootBoxInventory?> GetAsync(Guid id, CancellationToken cancellation = default) => await _context.BoxInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation);

        public async Task CreateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
        {
            LootBoxInventory inventory = template.ToEntity();

            await _context.BoxInventories.AddAsync(inventory, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
        {
            LootBoxInventory inventory = await _context.BoxInventories
                .FirstOrDefaultAsync(lbi => lbi.Id == template.Id, cancellation) ??
                throw new NotFoundException("Инвентарь не найден");

            LootBoxInventory inventoryNew = template.ToEntity();

            _context.Entry(inventory).CurrentValues.SetValues(inventoryNew);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            LootBoxInventory inventory = await _context.BoxInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation) ??
                throw new NotFoundException("Инвентарь не найден");

            _context.BoxInventories.Remove(inventory);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
