using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services;

public class LootBoxService(ApplicationDbContext context) : ILootBoxService
{
    public async Task<LootBox?> GetAsync(Guid id, CancellationToken cancellation = default) => 
        await context.Boxes
        .AsNoTracking()
        .FirstOrDefaultAsync(lb => lb.Id == id, cancellation);

    public async Task CreateAsync(LootBoxTemplate template, CancellationToken cancellation = default)
    {
        await context.Boxes.AddAsync(new LootBox
        {
            Id = template.Id,
            Cost = template.Cost,
            IsLocked = template.IsLocked,
        }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(LootBoxTemplate template, CancellationToken cancellation = default)
    {
        var old = await context.Boxes
            .FirstOrDefaultAsync(lb => lb.Id == template.Id, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        context.Entry(old).CurrentValues.SetValues(new LootBox
        {
            Id = template.Id,
            Cost = template.Cost,
            IsLocked = template.IsLocked,
            ExpirationBannerDate = old.ExpirationBannerDate,
            Balance = old.Balance,
            VirtualBalance = old.VirtualBalance
        });
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template, CancellationToken cancellation = default)
    {
        var box = await context.Boxes
            .FirstOrDefaultAsync(lb => lb.Id == template.BoxId, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        box.ExpirationBannerDate = template.ExpirationDate;

        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var box = await context.Boxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        context.Boxes.Remove(box);
        await context.SaveChangesAsync(cancellation);
    }
}