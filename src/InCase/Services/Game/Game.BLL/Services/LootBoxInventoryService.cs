using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services;
public class LootBoxInventoryService(ApplicationDbContext context) : ILootBoxInventoryService
{
    public async Task<LootBoxInventory?> GetAsync(Guid id, CancellationToken cancellation = default) => 
        await context.LootBoxInventories
        .AsNoTracking()
        .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation);

    public async Task CreateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
    {
        await context.LootBoxInventories.AddAsync(new LootBoxInventory
        {
            Id = template.Id,
            BoxId = template.BoxId,
            ChanceWining = template.ChanceWining,
            ItemId = template.ItemId,
        }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default)
    {
        var inventory = await context.LootBoxInventories
            .FirstOrDefaultAsync(lbi => lbi.Id == template.Id, cancellation) ??
            throw new NotFoundException("Инвентарь не найден");

        context.Entry(inventory).CurrentValues.SetValues(new LootBoxInventory
        {
            Id = template.Id,
            BoxId = template.BoxId,
            ChanceWining = template.ChanceWining,
            ItemId = template.ItemId,
        });
        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var inventory = await context.LootBoxInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(lbi => lbi.Id == id, cancellation) ??
            throw new NotFoundException("Инвентарь не найден");

        context.LootBoxInventories.Remove(inventory);
        await context.SaveChangesAsync(cancellation);
    }
}