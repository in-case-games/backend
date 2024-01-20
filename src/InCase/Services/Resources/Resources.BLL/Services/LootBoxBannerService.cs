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
    public class LootBoxBannerService(ApplicationDbContext context, BasePublisher publisher) : ILootBoxBannerService
    {
        public async Task<LootBoxBannerResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var banner = await context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync(CancellationToken cancellation = default)
        {
            var banners = await context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return banners.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync(bool isActive, CancellationToken cancellation = default)
        {
            var banners = await context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .Where(lbb => isActive ? DateTime.UtcNow <= lbb.ExpirationDate : DateTime.UtcNow > lbb.ExpirationDate)
                .ToListAsync(cancellation);

            return banners.ToResponse();
        }

        public async Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default)
        {
            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == id, cancellation))
                throw new NotFoundException("Кейс не найден");

            var banner = await context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.BoxId == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

            var box = await context.LootBoxes
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
                BoxId = request.BoxId
            };

            await context.Banners.AddAsync(banner, cancellation);
            await context.SaveChangesAsync(cancellation);

            banner.Box = box;

            await publisher.SendAsync(banner.ToTemplate(isDeleted: false), cancellation);

            FileService.UploadImageBase64(request.Image, $"loot-box-banners/{box.Id}/", $"{banner.Id}");

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> UpdateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default)
        {
            var oldBanner = await context.Banners
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

            context.Entry(oldBanner).CurrentValues.SetValues(newBanner);
            await context.SaveChangesAsync(cancellation);
            await publisher.SendAsync(newBanner.ToTemplate(isDeleted: false), cancellation);

            return newBanner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var banner = await context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id, cancellation) ??
                throw new NotFoundException("Баннер не найден");

            context.Banners.Remove(banner);
            await context.SaveChangesAsync(cancellation);
            await publisher.SendAsync(banner.ToTemplate(isDeleted: true), cancellation);

            FileService.RemoveFolder($"loot-box-banners/{banner.BoxId}/");

            return banner.ToResponse();
        }
    }
}
