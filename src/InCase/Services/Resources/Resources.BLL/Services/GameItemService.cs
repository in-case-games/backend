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
    public class GameItemService : IGameItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        private readonly Dictionary<string, IGamePlatformService> _platformServices;

        public GameItemService(
            ApplicationDbContext context,
            BasePublisher publisher,
            GamePlatformSteamService steamService)
        {
            _context = context;
            _publisher = publisher;

            _platformServices = new Dictionary<string, IGamePlatformService>
            {
                ["csgo"] = steamService,
                ["dota2"] = steamService,
            };
        }

        public async Task<GameItemResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var item = await _context.Items
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
            var items = await _context.Items
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
            if (!await _context.Games.AnyAsync(g => g.Id == id, cancellation))
                throw new NotFoundException("Игра не найдена");

            var items = await _context.Items
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
            var items = await _context.Items
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
            var items = await _context.Items
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
            if (!await _context.Qualities.AnyAsync(giq => giq.Name == name, cancellation))
                throw new NotFoundException("Качество не найдено");

            var items = await _context.Items
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
            if (!await _context.Rarities.AnyAsync(giq => giq.Name == name, cancellation))
                throw new NotFoundException("Редкость не найдено");

            var items = await _context.Items
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
            if (!await _context.ItemTypes.AnyAsync(giq => giq.Name == name, cancellation))
                throw new NotFoundException("Тип не найден");

            var items = await _context.Items
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
            await _context.Qualities
            .AsNoTracking()
            .ToListAsync(cancellation);

        public async Task<List<GameItemRarity>> GetRaritiesAsync(CancellationToken cancellation = default) =>
            await _context.Rarities
            .AsNoTracking()
            .ToListAsync(cancellation);

        public async Task<List<GameItemType>> GetTypesAsync(CancellationToken cancellation = default) =>
            await _context.ItemTypes
            .AsNoTracking()
            .ToListAsync(cancellation);

        public async Task<GameItemResponse> CreateAsync(GameItemRequest request, CancellationToken cancellation = default)
        {
            if (request.Cost <= 0) throw new BadRequestException("Предмет должен стоить больше 0");
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

            var quality = await _context.Qualities
                .AsNoTracking()
                .FirstOrDefaultAsync(giq => giq.Id == request.QualityId, cancellation) ??
                throw new NotFoundException("Качество предмета не найдено");
            var rarity = await _context.Rarities
                .AsNoTracking()
                .FirstOrDefaultAsync(gir => gir.Id == request.RarityId, cancellation) ??
                throw new NotFoundException("Редкость предмета не найдена");
            var type = await _context.ItemTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(git => git.Id == request.TypeId, cancellation) ??
                throw new NotFoundException("Тип предмета не найден");
            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellation) ??
                throw new NotFoundException("Игра не найдена");

            var item = request.ToEntity(true);

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            FileService.UploadImageBase64(request.Image, @$"game-items/{game.Id}/{item.Id}/", $"{item.Id}");

            await _context.Items.AddAsync(item, cancellation);
            await _publisher.SendAsync(item.ToTemplate(), cancellation);
            await _context.SaveChangesAsync(cancellation);

            return item.ToResponse();
        }

        public async Task<GameItemResponse> UpdateAsync(GameItemRequest request, CancellationToken cancellation = default)
        {
            if (request.Cost <= 0) throw new BadRequestException("Предмет должен стоить больше 0");

            var itemOld = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == request.Id, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            var quality = await _context.Qualities
                .AsNoTracking()
                .FirstOrDefaultAsync(giq => giq.Id == request.QualityId, cancellation) ??
                throw new NotFoundException("Качество предмета не найдено");
            var rarity = await _context.Rarities
                .AsNoTracking()
                .FirstOrDefaultAsync(gir => gir.Id == request.RarityId, cancellation) ??
                throw new NotFoundException("Редкость предмета не найдена");
            var type = await _context.ItemTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(git => git.Id == request.TypeId, cancellation) ??
                throw new NotFoundException("Тип предмета не найден");
            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellation) ??
                throw new NotFoundException("Игра не найдена");

            var item = request.ToEntity();

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            if (request.Image is not null)
            {
                FileService.UploadImageBase64(request.Image,
                    @$"game-items/{game.Id}/{item.Id}/", $"{item.Id}");
            }

            _context.Entry(itemOld).CurrentValues.SetValues(item);
            await _publisher.SendAsync(item.ToTemplate(), cancellation);
            await _context.SaveChangesAsync(cancellation);

            await CorrectCostAsync(item.Id, itemOld.Cost, cancellation);
            await CorrectChancesAsync(item.Id, cancellation);

            return item.ToResponse();
        }

        public async Task<GameItemResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var item = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            _context.Items.Remove(item);
            await _publisher.SendAsync(item!.ToTemplate(isDeleted: true), cancellation);
            await _context.SaveChangesAsync(cancellation);

            FileService.RemoveFolder(@$"game-items/{item.GameId}/{id}/");

            return item!.ToResponse();
        }

        public async Task UpdateCostManagerAsync(int count, CancellationToken cancellationToken = default)
        {
            var items = await _context.Items
                .Include(gi => gi.Game)
                .OrderByDescending(gi => gi.UpdateDate)
                .Take(count)
                .Where(gi => gi.UpdateDate + TimeSpan.FromMinutes(5) <= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                var priceOriginal = await _platformServices[item.Game!.Name!]
                    .GetOriginalMarketAsync(item.HashName ?? "null", item.Game!.Name!, cancellation: cancellationToken);
                var priceAdditional = await _platformServices[item.Game!.Name!]
                    .GetAdditionalMarketAsync(item.IdForMarket!, item.Game!.Name!, cancellation: cancellationToken);

                var isAdditionalCost = priceOriginal.Cost <= 0 || (priceAdditional.Cost > priceOriginal.Cost);
                var cost = isAdditionalCost ? priceAdditional.Cost : priceOriginal.Cost;

                if (cost <= 0) continue;

                item.UpdateDate = DateTime.UtcNow;
                item.Cost = cost * 7M;

                _context.Items.Update(item);
                await _publisher.SendAsync(item.ToTemplate(), cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await CorrectCostAsync(item.Id, item.Cost, cancellationToken);
                await CorrectChancesAsync(item.Id, cancellationToken);
            }
        }

        private async Task CorrectCostAsync(Guid itemId, decimal lastPriceItem, CancellationToken cancellationToken = default)
        {
            var inventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .Where(lbi => lbi.ItemId == itemId)
                .ToListAsync(cancellationToken);

            foreach (var box in inventories.Select(inventory => inventory.Box!))
            {
                var boxInventories = await _context.BoxInventories
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

                _context.LootBoxes.Update(box);

                await _publisher.SendAsync(box.ToTemplate(), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task CorrectChancesAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            var itemInventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .Where(lbi => lbi.ItemId == itemId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var itemInventory in itemInventories)
            {
                var weights = new Dictionary<Guid, decimal>();
                var boxInventories = await _context.BoxInventories
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

                    _context.BoxInventories.Update(boxInventory);
                    await _publisher.SendAsync(new LootBoxInventoryTemplate()
                    {
                        Id = boxInventory.Id,
                        BoxId = boxInventory.BoxId,
                        ChanceWining = boxInventory.ChanceWining,
                        ItemId = boxInventory.ItemId,
                    }, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
