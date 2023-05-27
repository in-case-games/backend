using Microsoft.EntityFrameworkCore;
using Resources.BLL.Entities;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class LootBoxBannerService : ILootBoxBannerService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxBannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LootBoxBannerResponse> GetAsync(Guid id)
        {
            LootBoxBanner banner = await _context.BoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<List<LootBoxBannerResponse>> GetAsync()
        {
            List<LootBoxBanner> banners = await _context.BoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .ToListAsync();

            return banners.ToResponse();
        }

        public async Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id)
        {
            if (!await _context.LootBoxes.AnyAsync(lb => lb.Id == id))
                throw new NotFoundException("Кейс не найден");

            LootBoxBanner banner = await _context.BoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.BoxId == id) ??
                throw new NotFoundException("Баннер не найден");

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request)
        {
            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            if (box.Banner != null) throw new ConflictException("Кейс уже использует баннер");

            LootBoxBanner banner = request.ToEntity(isNewGuid: true);

            await _context.BoxBanners.AddAsync(banner);
            await _context.SaveChangesAsync();

            banner.Box = box;

            //TODO Notify by rabbit mq

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> UpdateAsync(LootBoxBannerRequest request)
        {
            LootBoxBanner oldBanner = await _context.BoxBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == request.Id) ?? 
                throw new NotFoundException("Баннер не найден");

            LootBox box = await _context.LootBoxes
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            if (box.Banner != null && box.Banner.Id != request.Id) 
                throw new ConflictException("Кейс уже использует баннер");

            LootBoxBanner banner = request
                .ToEntity(isNewGuid: false, creationDate: oldBanner.CreationDate);

            _context.BoxBanners.Update(banner);
            await _context.SaveChangesAsync();

            banner.Box = box;

            //TODO Notify by rabbit mq

            return banner.ToResponse();
        }

        public async Task<LootBoxBannerResponse> DeleteAsync(Guid id)
        {
            LootBoxBanner banner = await _context.BoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == id) ??
                throw new NotFoundException("Баннер не найден");

            _context.BoxBanners.Remove(banner);
            await _context.SaveChangesAsync();

            //TODO Notify by rabbit mq

            return banner.ToResponse();
        }
    }
}
