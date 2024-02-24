using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services;
public class GameItemService(
    ILogger<GameItemService> logger, 
    ApplicationDbContext context, 
    BasePublisher publisher, 
    GamePlatformSteamService steamService) : IGameItemService
{
    private readonly Dictionary<string, IGamePlatformService> _platformServices = new()
    {
        ["csgo"] = steamService,
        ["dota2"] = steamService,
    };

    public async Task<GameItemResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var item = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        return item.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetAsync(string name, CancellationToken cancellation = default)
    {
        var items = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.Name == name)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default)
    {
        if (!await context.Games.AnyAsync(g => g.Id == id, cancellation))
            throw new NotFoundException("Игра не найдена");

        var items = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.GameId == id)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetByHashNameAsync(string hash, CancellationToken cancellation = default)
    {
        var items = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.HashName == hash)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetAsync(CancellationToken cancellation = default)
    {
        var items = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetByQualityAsync(string name, CancellationToken cancellation = default)
    {
        if (!await context.GameItemQualities.AnyAsync(giq => giq.Name == name, cancellation))
            throw new NotFoundException("Качество не найдено");

        var items = await context.GameItems
            .Include(gi => gi.Quality)
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.Quality!.Name == name)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetByRarityAsync(string name, CancellationToken cancellation = default)
    {
        if (!await context.GameItemRarities.AnyAsync(giq => giq.Name == name, cancellation))
            throw new NotFoundException("Редкость не найдено");

        var items = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.Rarity!.Name == name)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemResponse>> GetByTypeAsync(string name, CancellationToken cancellation = default)
    {
        if (!await context.GameItemTypes.AnyAsync(giq => giq.Name == name, cancellation))
            throw new NotFoundException("Тип не найден");

        var items = await context.GameItems
            .Include(gi => gi.Type)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .Where(gi => gi.Type!.Name == name)
            .ToListAsync(cancellation);

        return items.ToResponse();
    }

    public async Task<List<GameItemQuality>> GetQualitiesAsync(CancellationToken cancellation = default) =>
        await context.GameItemQualities
        .AsNoTracking()
        .ToListAsync(cancellation);

    public async Task<List<GameItemRarity>> GetRaritiesAsync(CancellationToken cancellation = default) =>
        await context.GameItemRarities
        .AsNoTracking()
        .ToListAsync(cancellation);

    public async Task<List<GameItemType>> GetTypesAsync(CancellationToken cancellation = default) =>
        await context.GameItemTypes
        .AsNoTracking()
        .ToListAsync(cancellation);

    public async Task<GameItemResponse> CreateAsync(GameItemRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsGameItem(request);

        if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

        var quality = await context.GameItemQualities
            .AsNoTracking()
            .FirstOrDefaultAsync(giq => giq.Id == request.QualityId, cancellation) ??
            throw new NotFoundException("Качество предмета не найдено");
        var rarity = await context.GameItemRarities
            .AsNoTracking()
            .FirstOrDefaultAsync(gir => gir.Id == request.RarityId, cancellation) ??
            throw new NotFoundException("Редкость предмета не найдена");
        var type = await context.GameItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(git => git.Id == request.TypeId, cancellation) ??
            throw new NotFoundException("Тип предмета не найден");
        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellation) ??
            throw new NotFoundException("Игра не найдена");

        var item = request.ToEntity(true);
        item.UpdateTo = DateTime.UtcNow.AddMinutes(10);
        item.UpdatedIn = DateTime.UtcNow;

        await context.GameItems.AddAsync(item, cancellation);
        await context.SaveChangesAsync(cancellation);

        item.Game = game;
        item.Quality = quality;
        item.Rarity = rarity;
        item.Type = type;

        await publisher.SendAsync(item.ToTemplate(), cancellation);

        FileService.UploadImageBase64(request.Image, $"game-items/{game.Id}/{item.Id}/", $"{item.Id}");

        return item.ToResponse();
    }

    public async Task<GameItemResponse> UpdateAsync(GameItemRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsGameItem(request);

        var itemOld = await context.GameItems
            .FirstOrDefaultAsync(gi => gi.Id == request.Id, cancellation) ??
            throw new NotFoundException("Предмет не найден");
        var quality = await context.GameItemQualities
            .AsNoTracking()
            .FirstOrDefaultAsync(giq => giq.Id == request.QualityId, cancellation) ??
            throw new NotFoundException("Качество предмета не найдено");
        var rarity = await context.GameItemRarities
            .AsNoTracking()
            .FirstOrDefaultAsync(gir => gir.Id == request.RarityId, cancellation) ??
            throw new NotFoundException("Редкость предмета не найдена");
        var type = await context.GameItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(git => git.Id == request.TypeId, cancellation) ??
            throw new NotFoundException("Тип предмета не найден");
        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellation) ??
            throw new NotFoundException("Игра не найдена");

        var item = request.ToEntity();
        item.UpdateTo = DateTime.UtcNow.AddMinutes(10);
        item.UpdatedIn = DateTime.UtcNow;

        context.Entry(itemOld).CurrentValues.SetValues(item);
        await context.SaveChangesAsync(cancellation);

        item.Game = game;
        item.Quality = quality;
        item.Rarity = rarity;
        item.Type = type;

        await publisher.SendAsync(item.ToTemplate(), cancellation);

        await CorrectCostAsync(item.Id, itemOld.Cost, cancellation);
        await CorrectChancesAsync(item.Id, cancellation);

        if (request.Image is not null)
        {
            FileService.UploadImageBase64(request.Image, $"game-items/{game.Id}/{item.Id}/", $"{item.Id}");
        }

        return item.ToResponse();
    }

    public async Task<GameItemResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var item = await context.GameItems
            .Include(gi => gi.Rarity)
            .Include(gi => gi.Quality)
            .Include(gi => gi.Type)
            .Include(gi => gi.Game)
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        context.GameItems.Remove(item);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(item.ToTemplate(isDeleted: true), cancellation);

        FileService.RemoveFolder($"game-items/{item.GameId}/{id}/");

        return item!.ToResponse();
    }

    public async Task UpdateCostManagerAsync(CancellationToken cancellationToken = default)
    {
        var item = await context.GameItems
            .Include(gi => gi.Game)
            .OrderByDescending(gi => gi.UpdateTo)
            .Where(gi => gi.UpdateTo <= DateTime.UtcNow)
            .OrderByDescending(gi => gi.UpdateTo)
            .FirstOrDefaultAsync(cancellationToken);

        if(item is null) return;

        var priceOriginal = await _platformServices[item.Game!.Name!]
            .GetOriginalMarketAsync(item.HashName ?? "null", item.Game!.Name!, cancellation: cancellationToken);
        var priceAdditional = await _platformServices[item.Game!.Name!]
            .GetAdditionalMarketAsync(item.IdForMarket!, item.Game!.Name!, cancellation: cancellationToken);

        var isAdditionalCost = priceOriginal.Cost <= 0 || priceAdditional.Cost > priceOriginal.Cost;
        var cost = isAdditionalCost ? priceAdditional.Cost : priceOriginal.Cost;

        item.UpdateTo = DateTime.UtcNow.AddHours(1);
        item.UpdatedIn = DateTime.UtcNow;

        if (cost > 0) item.Cost = cost <= 1 ? 7 : cost * 7M;

        context.GameItems.Update(item);
        await context.SaveChangesAsync(cancellationToken);

        if (cost <= 0)
        {
            logger.LogError($"ItemId - {item.Id} не смог получить цену. " +
                             $"Цена маркет - {priceOriginal}; Цена доп маркета - {priceAdditional}");
            return;
        }

        await publisher.SendAsync(item.ToTemplate(), cancellationToken);

        await CorrectCostAsync(item.Id, item.Cost, cancellationToken);
        await CorrectChancesAsync(item.Id, cancellationToken);
    }

    private async Task CorrectCostAsync(Guid itemId, decimal lastPriceItem, CancellationToken cancellationToken = default)
    {
        var inventories = await context.LootBoxInventories
            .Include(lbi => lbi.Box)
            .Where(lbi => lbi.ItemId == itemId)
            .ToListAsync(cancellationToken);

        foreach (var box in inventories.Select(inventory => inventory.Box!))
        {
            try
            {
                var boxInventories = await context.LootBoxInventories
                    .Include(lbi => lbi.Item)
                    .OrderBy(lbi => lbi.Item!.Cost)
                    .AsNoTracking()
                    .Where(lbi => lbi.BoxId == box.Id)
                    .ToListAsync(cancellationToken);

                var itemMinCost = boxInventories[0].Item!.Cost;
                var itemTwoCost = boxInventories.FirstOrDefault(lbi => lbi.ItemId != itemId)?.Item?.Cost ?? 0;
                var itemMaxCost = boxInventories[^1].Item!.Cost;

                var boxCostNew = itemMinCost * (box.Cost / itemMinCost);

                if (boxInventories[0].Item!.Id == itemId)
                {
                    if (lastPriceItem < box.Cost) boxCostNew = itemMinCost * (box.Cost / lastPriceItem);
                    else if (itemTwoCost != 0) boxCostNew = itemMinCost * (box.Cost / itemTwoCost);
                }

                box.IsLocked = boxCostNew >= itemMaxCost || boxCostNew <= itemMinCost;
                box.Cost = box.IsLocked ? box.Cost : boxCostNew;

                context.LootBoxes.Update(box);
                await context.SaveChangesAsync(cancellationToken);
                await publisher.SendAsync(box.ToTemplate(), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"BoxId - {box.Id}; ItemId - {itemId}; не смог обновить стоимость кейса");
                logger.LogCritical(ex, ex.Message);
                logger.LogCritical(ex.StackTrace);

                box.IsLocked = true;
                context.LootBoxes.Update(box);
                await context.SaveChangesAsync(cancellationToken);
                await publisher.SendAsync(box.ToTemplate(), cancellationToken);

                logger.LogCritical($"BoxId - {box.Id} заблокирован");
            }
        }
    }

    private async Task CorrectChancesAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        var itemInventories = await context.LootBoxInventories
            .Include(lbi => lbi.Box)
            .Where(lbi => lbi.ItemId == itemId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var itemInventory in itemInventories)
        {
            try
            {
                var weights = new Dictionary<Guid, decimal>();
                var boxInventories = await context.LootBoxInventories
                    .Include(lbi => lbi.Item)
                    .Where(lbi => lbi.BoxId == itemInventory.Box!.Id)
                    .ToListAsync(cancellationToken);

                decimal weightAll = 0;

                foreach (var boxInventory in boxInventories)
                {
                    var weight = 1M / boxInventory.Item!.Cost;

                    weightAll += weight;
                    weights.Add(boxInventory.Id, weight);
                }

                foreach (var boxInventory in boxInventories)
                {
                    boxInventory.ChanceWining = decimal.ToInt32(
                        Math.Round(weights[boxInventory.Id] / weightAll * 10000000M));

                    context.LootBoxInventories.Update(boxInventory);
                    await context.SaveChangesAsync(cancellationToken);
                    await publisher.SendAsync(new LootBoxInventoryTemplate()
                    {
                        Id = boxInventory.Id,
                        BoxId = boxInventory.BoxId,
                        ChanceWining = boxInventory.ChanceWining,
                        ItemId = boxInventory.ItemId,
                    }, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical($"InventoryId - {itemInventory.Id}; ItemId - {itemId}; не смог обновить шансы");
                logger.LogCritical(ex, ex.Message);
                logger.LogCritical(ex.StackTrace);
            }
        }
    }
}