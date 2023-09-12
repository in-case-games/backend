using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;
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
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> GetAsync(string name)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Name == name) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetAsync()
        {
            List<LootBox> boxes = await _context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id)
        {
            if (!await _context.Games.AnyAsync(g => g.Id == id))
                throw new NotFoundException("Игра не найдена");

            List<LootBox> boxes = await _context.LootBoxes
                .AsNoTracking()
                .Where(lb => lb.GameId == id)
                .ToListAsync();

            return boxes.ToResponse();
        }

        public async Task<LootBoxResponse> CreateAsync(LootBoxRequest request,
            IFormFile uploadImage)
        {
            if (request.Cost <= 0) 
                throw new BadRequestException("Кейс должен стоить больше 0");
            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId))
                throw new NotFoundException("Игра не найдена");
            if (await _context.LootBoxes.AnyAsync(lb => lb.Name == request.Name)) 
                throw new ConflictException("Название кейса уже занято");

            request.IsLocked = true;

            LootBox box = request.ToEntity(isNewGuid: true);

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            FileService.Upload(uploadImage,
                "loot-boxes\\{request.GameId}\\{box.Id}\\" + box.Id + ".jpg");

            await _context.LootBoxes.AddAsync(box);
            await _context.SaveChangesAsync();

            

            await _publisher.SendAsync(box.ToTemplate(isDeleted: false));

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> UpdateAsync(LootBoxRequest request,
            IFormFile? uploadImage)
        {
            LootBox oldBox = await _context.LootBoxes
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

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            if (uploadImage is not null)
            {
                string filePath = "loot-boxes\\{request.GameId}\\{newBox.Id}\\" + newBox.Id + ".jpg";
                File.Delete(filePath);
                FileService.RemoveFolder("loot-boxes\\{request.GameId}\\{newBox.Id}\\");

                FileService.Upload(uploadImage, filePath);
            }

            _context.Entry(oldBox).CurrentValues.SetValues(newBox);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(newBox.ToTemplate(isDeleted: false));

            return newBox.ToResponse();
        }

        public async Task<LootBoxResponse> DeleteAsync(Guid id)
        {
            LootBox box = await _context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            _context.LootBoxes.Remove(box);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(box.ToTemplate(isDeleted: true));

            return box.ToResponse();
        }
    }
}
