using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Resources.BLL.Entities;
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

        public async Task<LootBoxBannerResponse> GetAsync(Guid id)
        {
            LootBoxBanner banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync()
        {
            List<LootBoxBanner> banners = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .ToListAsync();

            return banners.ToResponse();
        }

        public async Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id)
        {
            if (!await _context.LootBoxes.AnyAsync(lb => lb.Id == id))
                throw new NotFoundException("Кейс не найден");

            LootBoxBanner banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.BoxId == id) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите фото в base 64");

            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            LootBoxBanner banner = request.ToEntity(isNewGuid: true);

            if (box.Banner != null) 
                throw new ConflictException("Кейс уже использует баннер");

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            FileService.Upload(request.Image,
                "loot-box-banners\\{box.Id}\\{banner.Id}\\" + banner.Id + ".jpg");

            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(banner.ToTemplate(isDeleted: false));

            banner.Box = box;

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> UpdateAsync(LootBoxBannerRequest request)
        {
            LootBoxBanner bannerOld = await _context.Banners
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == request.Id) ?? 
                throw new NotFoundException("Баннер не найден");

            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            LootBoxBanner banner = request
                .ToEntity(isNewGuid: false, creationDate: bannerOld.CreationDate);

            if (box.Banner != null && box.Banner.Id != request.Id) 
                throw new ConflictException("Кейс уже использует баннер");

            if (request.Image is not null)
            {
                string[] currentDirPath = Environment.CurrentDirectory.Split("src");
                string path = currentDirPath[0];

                string filePath = "loot-box-banners\\{box.Id}\\{banner.Id}\\" + banner.Id + ".jpg";
                File.Delete(filePath);
                FileService.RemoveFolder("loot-box-banners\\{box.Id}\\{banner.Id}\\");

                FileService.Upload(request.Image, filePath);
            }

            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(banner.ToTemplate(isDeleted: false));

            banner.Box = box;

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> DeleteAsync(Guid id)
        {
            LootBoxBanner banner = await _context.Banners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id) ??
                throw new NotFoundException("Баннер не найден");

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(banner.ToTemplate(isDeleted: true));

            return banner.ToResponse();
        }
    }
}
