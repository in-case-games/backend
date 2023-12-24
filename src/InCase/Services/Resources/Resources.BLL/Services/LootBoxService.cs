﻿using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.MassTransit;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

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

        public async Task<LootBoxResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> GetAsync(string name, CancellationToken cancellation = default)
        {
            var box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Name == name, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            return box.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetAsync(CancellationToken cancellation = default)
        {
            var boxes = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return boxes.ToResponse();
        }

        public async Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default)
        {
            if (!await _context.Games.AnyAsync(g => g.Id == id, cancellationToken: cancellation))
                throw new NotFoundException("Игра не найдена");

            var boxes = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .Where(lb => lb.GameId == id)
                .ToListAsync(cancellation);

            return boxes.ToResponse();
        }

        public async Task<LootBoxResponse> CreateAsync(LootBoxRequest request, CancellationToken cancellation = default)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base 64");
            if (request.Cost <= 0) throw new BadRequestException("Кейс должен стоить больше 0");

            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId, cancellation))
                throw new NotFoundException("Игра не найдена");
            if (await _context.LootBoxes.AnyAsync(lb => lb.Name == request.Name, cancellation)) 
                throw new ConflictException("Название кейса уже занято");

            request.IsLocked = true;

            var box = new LootBox
            {
                Id = Guid.NewGuid(),
                GameId = request.GameId,
                Name = request.Name,
                IsLocked = request.IsLocked,
                Cost = request.Cost,
            };

            await _context.LootBoxes.AddAsync(box, cancellation);
            await _context.SaveChangesAsync(cancellation);
            
            await _publisher.SendAsync(box.ToTemplate(isDeleted: false), cancellation);

            FileService.UploadImageBase64(request.Image, $"loot-boxes/{box.Id}/", $"{box.Id}");
            FileService.CreateFolder(@$"loot-box-banners/{box.Id}/");

            return box.ToResponse();
        }

        public async Task<LootBoxResponse> UpdateAsync(LootBoxRequest request, CancellationToken cancellation = default)
        {
            var oldBox = await _context.LootBoxes
                .Include(lb => lb.Game)
                .FirstOrDefaultAsync(lb => lb.Id == request.Id, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            if (request.Cost <= 0) throw new BadRequestException("Кейс должен стоить больше 0");

            if (!await _context.Games.AnyAsync(g => g.Id == request.GameId, cancellation))
                throw new NotFoundException("Игра не найдена");
            if (await _context.LootBoxes.AnyAsync(lb => lb.Name == request.Name && lb.Id != request.Id, cancellation))
                throw new ConflictException("Название кейса уже занято");

            var boxInventories = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .OrderBy(lbi => lbi.Item!.Cost)
                .AsNoTracking()
                .Where(lbi => lbi.BoxId == request.Id)
                .ToListAsync(cancellation);

            var isLocked = boxInventories.Count < 1 || 
                boxInventories[0].Item!.Cost > request.Cost ||
                boxInventories[^1].Item!.Cost < request.Cost;

            request.IsLocked = isLocked || request.IsLocked;

            var newBox = new LootBox
            {
                Id = request.Id,
                GameId = request.GameId,
                Name = request.Name,
                IsLocked = request.IsLocked,
                Cost = request.Cost,
            };

            _context.Entry(oldBox).CurrentValues.SetValues(newBox);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(newBox.ToTemplate(isDeleted: false), cancellation);

            if (request.Image is not null)
            {
                FileService.UploadImageBase64(request.Image, @$"loot-boxes/{request.Id}/", $"{request.Id}");
            }

            return newBox.ToResponse();
        }

        public async Task<LootBoxResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var box = await _context.LootBoxes
                .Include(lb => lb.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            _context.LootBoxes.Remove(box);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(box.ToTemplate(isDeleted: true), cancellation);

            FileService.RemoveFolder($"loot-boxes/{box.Id}/");
            FileService.RemoveFolder($"loot-box-banners/{box.Id}/");

            return box.ToResponse();
        }
    }
}
