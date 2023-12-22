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
    public class LootBoxBannerService : ILootBoxBannerService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxBannerService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<LootBoxBannerResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync(CancellationToken cancellation = default)
        {
            var banners = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return banners.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync(bool isActive, CancellationToken cancellation = default)
        {
            var banners = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .Where(lbb => isActive ? DateTime.UtcNow <= lbb.ExpirationDate : DateTime.UtcNow > lbb.ExpirationDate)
                .ToListAsync(cancellation);

            return banners.ToResponse();
        }

        public async Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default)
        {
            if (!await _context.LootBoxes.AnyAsync(lb => lb.Id == id, cancellation))
                throw new NotFoundException("Кейс не найден");

            var banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.BoxId == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

            var box = await _context.LootBoxes
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            if (box.Banner != null) throw new ConflictException("Кейс уже использует баннер");

            var banner = new LootBoxBanner()
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                ExpirationDate = request.ExpirationDate,
                BoxId = request.BoxId,
                Box = box
            };

            FileService.UploadImageBase64(request.Image, $"loot-box-banners/{box.Id}/", $"{banner.Id}");

            await _context.Banners.AddAsync(banner, cancellation);
            await _publisher.SendAsync(banner.ToTemplate(isDeleted: false), cancellation);
            await _context.SaveChangesAsync(cancellation);

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> UpdateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default)
        {
            var oldBanner = await _context.Banners
                .Include(lbb => lbb.Box)
                .FirstOrDefaultAsync(lbb => lbb.Id == request.Id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            if (oldBanner.BoxId != request.BoxId) throw new BadRequestException("Кейс нельзя поменять");
            if (request.Image is not null)
            {
                FileService.UploadImageBase64(request.Image, $"loot-box-banners/{request.BoxId}/", $"{request.Id}");
            }

            var newBanner = new LootBoxBanner()
            {
                Id = request.Id,
                CreationDate = oldBanner.CreationDate,
                ExpirationDate = request.ExpirationDate,
                BoxId = request.BoxId,
            };

            _context.Entry(oldBanner).CurrentValues.SetValues(newBanner);
            await _publisher.SendAsync(newBanner.ToTemplate(isDeleted: false), cancellation);
            await _context.SaveChangesAsync(cancellation);

            return newBanner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            _context.Banners.Remove(banner);
            await _publisher.SendAsync(banner.ToTemplate(isDeleted: true), cancellation);
            await _context.SaveChangesAsync(cancellation);

            FileService.RemoveFolder($"loot-box-banners/{banner.BoxId}/");

            return banner.ToResponse();
        }
    }
}
