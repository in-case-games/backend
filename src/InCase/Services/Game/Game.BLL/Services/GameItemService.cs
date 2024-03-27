using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services;
public class GameItemService(ApplicationDbContext context) : IGameItemService
{
    public async Task<GameItem?> GetAsync(Guid id, CancellationToken cancellation = default) =>
        await context.GameItems
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation);

    public async Task CreateAsync(GameItemTemplate template, CancellationToken cancellation = default)
    {
        await context.GameItems.AddAsync(new GameItem
        {
            Id = template.Id,
            Cost = template.Cost
        }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(GameItemTemplate template, CancellationToken cancellation = default)
    {
        var item = await context.GameItems
                .FirstOrDefaultAsync(gi => gi.Id == template.Id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        item.Cost = template.Cost;

        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var item = await context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        context.GameItems.Remove(item);
        await context.SaveChangesAsync(cancellation);
    }
}