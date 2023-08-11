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

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            await _publisher.SendAsync(item.ToTemplate(request.IdForMarket));

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

            _context.Entry(itemOld).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();

            item.Game = game;
            item.Quality = quality;
            item.Rarity = rarity;
            item.Type = type;

            await _publisher.SendAsync(item.ToTemplate(request.IdForMarket));

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

            await _publisher.SendAsync(item.ToTemplate(idForMarket: null, isDeleted: true));

            return item.ToResponse();
        }

        public async Task UpdateCostManagerAsync(int count, CancellationToken cancellationToken)
        {
            List<GameItem> items = await _context.Items
                .Include(gi => gi.Game)
                .OrderByDescending(gi => gi.UpdateDate)
                .Take(count)
                .ToListAsync();

            foreach(GameItem item in items)
            {
                string game = item.Game!.Name!;

                decimal cost = await _platformServices[game].GetItemCostAsync(item.HashName!, game);

                item.UpdateDate = DateTime.UtcNow;
                item.Cost = cost * 7;

                _context.Items.Update(item);
                await _context.SaveChangesAsync(cancellationToken);
                await _publisher.SendAsync(item.ToTemplate(null));
            }
        }
    }
}
