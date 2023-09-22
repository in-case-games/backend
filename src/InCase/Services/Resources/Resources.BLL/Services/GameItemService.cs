using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;
using static System.Net.Mime.MediaTypeNames;

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

            _platformServices = new()
            {
                ["csgo"] = steamService,
                ["dota2"] = steamService,
            };
        }

        public async Task<GameItemResponse> GetAsync(Guid id)
        {
            GameItem item = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            return item.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetAsync(string name)
        {
            List<GameItem> items = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.Name == name)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetByGameIdAsync(Guid id)
        {
            if (!await _context.Games.AnyAsync(g => g.Id == id))
                throw new NotFoundException("Игра не найдена");

            List<GameItem> items = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.GameId == id)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetByHashNameAsync(string hash)
        {
            List<GameItem> items = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.HashName == hash)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetAsync()
        {
            List<GameItem> items = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetByQualityAsync(string name)
        {
            if (!await _context.Qualities.AnyAsync(giq => giq.Name == name))
                throw new NotFoundException("Качество не найдено");

            List<GameItem> items = await _context.Items
                .Include(gi => gi.Quality)
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.Quality!.Name == name)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetByRarityAsync(string name)
        {
            if (!await _context.Rarities.AnyAsync(giq => giq.Name == name))
                throw new NotFoundException("Редкость не найдено");

            List<GameItem> items = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.Rarity!.Name == name)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemResponse>> GetByTypeAsync(string name)
        {
            if (!await _context.ItemTypes.AnyAsync(giq => giq.Name == name))
                throw new NotFoundException("Тип не найден");

            List<GameItem> items = await _context.Items
                .Include(gi => gi.Type)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .Where(gi => gi.Type!.Name == name)
                .ToListAsync();

            return items.ToResponse();
        }

        public async Task<List<GameItemQuality>> GetQualitiesAsync() =>
            await _context.Qualities
            .AsNoTracking()
            .ToListAsync();

        public async Task<List<GameItemRarity>> GetRaritiesAsync() =>
            await _context.Rarities
            .AsNoTracking()
            .ToListAsync();

        public async Task<List<GameItemType>> GetTypesAsync() =>
            await _context.ItemTypes
            .AsNoTracking()
            .ToListAsync();

        public async Task<GameItemResponse> CreateAsync(GameItemRequest request)
        {
            if (request.Cost <= 0) throw new BadRequestException("Предмет должен стоить больше 0");
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

            GameItemQuality quality = await _context.Qualities
                .AsNoTracking()
                .FirstOrDefaultAsync(giq => giq.Id == request.QualityId) ??
                throw new NotFoundException("Качество предмета не найдено");
            GameItemRarity rarity = await _context.Rarities
                .AsNoTracking()
                .FirstOrDefaultAsync(gir => gir.Id == request.RarityId) ??
                throw new NotFoundException("Редкость предмета не найдена");
            GameItemType type = await _context.ItemTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(git => git.Id == request.TypeId) ??
                throw new NotFoundException("Тип предмета не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId) ??
                throw new NotFoundException("Игра не найдена");

            GameItem item = request.ToEntity(true);

            FileService.UploadImageBase64(request.Image, 
                @$"game-items/{game.Id}/{item.Id}/", $"{item.Id}");

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            await _publisher.SendAsync(item.ToTemplate());

            return item.ToResponse();
        }

        public async Task<GameItemResponse> UpdateAsync(GameItemRequest request)
        {
            if (request.Cost <= 0)
                throw new BadRequestException("Предмет должен стоить больше 0");

            GameItem itemOld = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == request.Id) ??
                throw new NotFoundException("Предмет не найден");

            GameItemQuality quality = await _context.Qualities
                .AsNoTracking()
                .FirstOrDefaultAsync(giq => giq.Id == request.QualityId) ??
                throw new NotFoundException("Качество предмета не найдено");
            GameItemRarity rarity = await _context.Rarities
                .AsNoTracking()
                .FirstOrDefaultAsync(gir => gir.Id == request.RarityId) ??
                throw new NotFoundException("Редкость предмета не найдена");
            GameItemType type = await _context.ItemTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(git => git.Id == request.TypeId) ??
                throw new NotFoundException("Тип предмета не найден");
            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId) ??
                throw new NotFoundException("Игра не найдена");

            GameItem item = request.ToEntity();

            if (request.Image is not null)
            {
                FileService.UploadImageBase64(request.Image,
                    @$"game-items/{game.Id}/{item.Id}/", $"{item.Id}");
            }

            _context.Entry(itemOld).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            await _publisher.SendAsync(item.ToTemplate());

            await CorrectCostAsync(item.Id, itemOld.Cost);
            await CorrectChancesAsync(item.Id);

            return item.ToResponse();
        }

        public async Task<GameItemResponse> DeleteAsync(Guid id)
        {
            GameItem item = await _context.Items
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Type)
                .Include(gi => gi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            _context.Items.Remove(item);

            await _context.SaveChangesAsync();
            await _publisher.SendAsync(item!.ToTemplate(isDeleted: true));

            FileService.RemoveFolder(@$"game-items/{item.GameId}/{id}/");

            return item!.ToResponse();
        }

        public async Task UpdateCostManagerAsync(int count, CancellationToken cancellationToken = default)
        {
            List<GameItem> items = await _context.Items
                .Include(gi => gi.Game)
                .OrderByDescending(gi => gi.UpdateDate)
                .Take(count)
                .Where(gi => gi.UpdateDate + TimeSpan.FromMinutes(5) <= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (GameItem item in items)
            {
                string game = item.Game!.Name!;
                string hashName = item.HashName ?? "null";
                decimal tempPrice = item.Cost;

                ItemCostResponse priceOriginal = await _platformServices[game]
                    .GetOriginalMarketAsync(hashName, game);
                ItemCostResponse priceAdditional = await _platformServices[game]
                    .GetAdditionalMarketAsync(item.IdForMarket!, game);

                bool isAdditionalCost = priceOriginal.Cost <= 0 ||
                    (priceAdditional.Cost > priceOriginal.Cost);
                decimal cost = isAdditionalCost ? priceAdditional.Cost : priceOriginal.Cost;


                if (cost > 0)
                {
                    item.UpdateDate = DateTime.UtcNow;
                    item.Cost = cost * 7M;

                    _context.Items.Update(item);
                    await _context.SaveChangesAsync(cancellationToken);

                    await _publisher.SendAsync(item.ToTemplate(), cancellationToken);

                    await CorrectCostAsync(item.Id, tempPrice, cancellationToken);
                    await CorrectChancesAsync(item.Id, cancellationToken);
                }
            }
        }

        private async Task CorrectCostAsync(
            Guid itemId,
            decimal lastPriceItem,
            CancellationToken cancellationToken = default)
        {
            List<LootBoxInventory> inventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .Where(lbi => lbi.ItemId == itemId)
                .ToListAsync(cancellationToken);

            foreach (var inventory in inventories)
            {
                LootBox box = inventory.Box!;

                List<LootBoxInventory> boxInventories = await _context.BoxInventories
                    .Include(lbi => lbi.Item)
                    .OrderBy(lbi => lbi.Item!.Cost)
                    .AsNoTracking()
                    .Where(lbi => lbi.BoxId == box.Id)
                    .ToListAsync(cancellationToken);

                decimal itemMinCost = boxInventories[0].Item!.Cost;
                decimal itemTwoCost = boxInventories
                    .FirstOrDefault(lbi => lbi.ItemId != itemId)?.Item?.Cost ?? 0;
                decimal itemMaxCost = boxInventories[^1].Item!.Cost;

                decimal boxCostNew = itemMinCost * (box.Cost / itemMinCost);

                if (boxInventories[0].Item!.Id == itemId)
                {
                    if (lastPriceItem < box.Cost)
                        boxCostNew = itemMinCost * (box.Cost / lastPriceItem);
                    else if (itemTwoCost != 0)
                        boxCostNew = itemMinCost * (box.Cost / itemTwoCost);
                }

                box.IsLocked = boxCostNew >= itemMaxCost || boxCostNew <= itemMinCost;

                box.Cost = box.IsLocked ? box.Cost : boxCostNew;

                _context.LootBoxes.Update(box);

                await _publisher.SendAsync(box.ToTemplate(), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task CorrectChancesAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            List<LootBoxInventory> itemInventories = await _context.BoxInventories
                .Include(lbi => lbi.Box)
                .Where(lbi => lbi.ItemId == itemId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var itemInventory in itemInventories)
            {
                LootBox box = itemInventory.Box!;
                decimal weightAll = 0;
                Dictionary<Guid, decimal> weights = new();

                List<LootBoxInventory> boxInventories = await _context.BoxInventories
                    .Include(lbi => lbi.Item)
                    .Where(lbi => lbi.BoxId == box.Id)
                    .ToListAsync(cancellationToken);

                foreach (var boxInventory in boxInventories)
                {
                    decimal weight = 1M / boxInventory.Item!.Cost;

                    weightAll += weight;
                    weights.Add(boxInventory.Id, weight);
                }

                foreach (var boxInventory in boxInventories)
                {
                    boxInventory.ChanceWining = decimal.ToInt32(
                        Math.Round(weights[boxInventory.Id] / weightAll * 10000000M));

                    _context.BoxInventories.Update(boxInventory);
                    await _publisher.SendAsync(boxInventory.ToTemplate(), cancellationToken);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
