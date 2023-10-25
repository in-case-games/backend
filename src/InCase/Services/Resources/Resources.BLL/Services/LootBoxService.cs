using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;
using System.Reflection;
using System.Threading;

namespace Resources.BLL.Services
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

        public async Task<LootBoxResponse> GetAsync(Guid id)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> GetAsync(string name)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Name == name) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetAsync()
        {
            List<LootBox> boxes = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id)
        {
            if (!await _context.Games.AnyAsync(g => g.Id == id))
                throw new NotFoundException("Игра не найдена");

            List<LootBox> boxes = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .Where(lb => lb.GameId == id)
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<LootBoxResponse> CreateAsync(LootBoxRequest request)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base 64");
            if (request.Cost <= 0) 
                throw new BadRequestException("Кейс должен стоить больше 0");
            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId))
                throw new NotFoundException("Игра не найдена");
            if (await _context.LootBoxes.AnyAsync(lb => lb.Name == request.Name)) 
                throw new ConflictException("Название кейса уже занято");

            request.IsLocked = true;

            LootBox box = request.ToEntity(isNewGuid: true);

            FileService.UploadImageBase64(request.Image,
                @$"loot-boxes/{box.Id}/", $"{box.Id}");
            FileService.CreateFolder(@$"loot-box-banners/{box.Id}/");

            await _context.LootBoxes.AddAsync(box);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(box.ToTemplate(isDeleted: false));

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> UpdateAsync(LootBoxRequest request)
        {
            LootBox oldBox = await _context.LootBoxes
                .Include(lb => lb.Game)
                .FirstOrDefaultAsync(lb => lb.Id == request.Id) ??
                throw new NotFoundException("Кейс не найден");

            if (request.Cost <= 0)
                throw new BadRequestException("Кейс должен стоить больше 0");
            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId))
                throw new NotFoundException("Игра не найдена");
            if (await _context.LootBoxes.AnyAsync(lb => lb.Name == request.Name && lb.Id != request.Id))
                throw new ConflictException("Название кейса уже занято");

            List<LootBoxInventory> boxInventories = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .OrderBy(lbi => lbi.Item!.Cost)
                .AsNoTracking()
                .Where(lbi => lbi.BoxId == request.Id)
                .ToListAsync();

            bool isLocked = boxInventories.Count < 1 || 
                boxInventories[0].Item!.Cost > request.Cost ||
                boxInventories[^1].Item!.Cost < request.Cost;

            request.IsLocked = isLocked || request.IsLocked;

            LootBox newBox = request.ToEntity(isNewGuid: false);

            if (request.Image is not null)
            {
                FileService.UploadImageBase64(request.Image,
                    @$"loot-boxes/{request.Id}/", $"{request.Id}");
            }

            _context.Entry(oldBox).CurrentValues.SetValues(newBox);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(newBox.ToTemplate(isDeleted: false));

            return newBox.ToResponse();
        }

        public async Task<LootBoxResponse> DeleteAsync(Guid id)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            _context.LootBoxes.Remove(box);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(box.ToTemplate(isDeleted: true));

            FileService.RemoveFolder(@$"loot-boxes/{box.Id}/");
            FileService.RemoveFolder(@$"loot-box-banners/{box.Id}/");

            return box.ToResponse();
        }
    }
}
