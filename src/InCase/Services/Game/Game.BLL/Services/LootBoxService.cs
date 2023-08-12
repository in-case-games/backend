using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class LootBoxService : ILootBoxService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<LootBox?> GetAsync(Guid id) => await _context.Boxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id);

        public async Task<List<LootBox>> CorrectCostAsync(Guid itemId)
        {
            GameItem item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == itemId) ??
                throw new NotFoundException("Предмет не найден");

            List<LootBoxInventory> inventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .Where(lbi => lbi.ItemId == itemId)
                .ToListAsync();

            List<LootBox> boxesUpdated = new(); 

            foreach (var inventory in inventories)
            {
                LootBox box = inventory.Box!;

                if (box.Cost < item.Cost)
                {
                    LootBoxInventory? itemSmallCost = await _context.BoxInventories
                        .Include(lbi => lbi.Item)
                        .OrderBy(lbi => lbi.Item!.Cost)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(lbi => lbi.BoxId == box.Id);

                    if (itemSmallCost is not null && itemSmallCost.Item!.Cost > box.Cost)
                    {
                        box.Cost = itemSmallCost!.Item!.Cost * 2M;
                        boxesUpdated.Add(box);

                        _context.Boxes.Update(box);

                        await _publisher.SendAsync(new LootBoxBackTemplate()
                        {
                            Id = box.Id,
                            Cost = box.Cost,
                            IsLocked = box.IsLocked,
                        });
                    }
                }
            }
            
            await _context.SaveChangesAsync();

            return boxesUpdated;
        }

        public async Task CorrectChancesAsync(List<LootBox> boxes)
        {
            foreach(var box in boxes)
            {
                decimal weightAll = 0;
                Dictionary<Guid, decimal> weights = new();

                List<LootBoxInventory> inventories = await _context.BoxInventories
                    .Include(lbi => lbi.Item)
                    .Where(lbi => lbi.BoxId == box.Id)
                    .ToListAsync();

                foreach(var inventory in inventories)
                {
                    decimal weight = 1M / inventory.Item!.Cost;

                    weightAll += weight;
                    weights.Add(inventory.Id, weight);
                }

                foreach(var inventory in inventories)
                {
                    inventory.ChanceWining = decimal.ToInt32(Math.Round(weights[inventory.Id] / weightAll * 10000000M));

                    _context.BoxInventories.Update(inventory);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(LootBoxTemplate template)
        {
            LootBox box = template.ToEntity();

            await _context.Boxes.AddAsync(box);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LootBoxTemplate template)
        {
            LootBox boxOld = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.Id) ??
                throw new NotFoundException("Кейс не найден");

            LootBox boxNew = template.ToEntity();

            _context.Entry(boxOld).CurrentValues.SetValues(boxNew);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template)
        {
            LootBox box = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            box.ExpirationBannerDate = template.ExpirationDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            LootBox box = await _context.Boxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
        }
    }
}
