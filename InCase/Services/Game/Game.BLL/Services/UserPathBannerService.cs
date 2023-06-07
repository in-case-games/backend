using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;

namespace Game.BLL.Services
{
    public class UserPathBannerService : IUserPathBannerService
    {
        private readonly ApplicationDbContext _context;

        public UserPathBannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserPathBannerResponse>> GetByUserIdAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserPathBanner> banners = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .Where(upb => upb.UserId == userId)
                .ToListAsync();

            return banners.ToResponse();
        }

        public async Task<List<UserPathBannerResponse>> GetByItemIdAsync(Guid itemId)
        {
            if (!await _context.GameItems.AnyAsync(gi => gi.Id == itemId))
                throw new NotFoundException("Предмет не найден");

            List<UserPathBanner> banners = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .Where(upb => upb.ItemId == itemId)
                .ToListAsync();

            return banners.ToResponse();
        }

        public async Task<UserPathBannerResponse> GetByIdAsync(Guid id)
        {
            UserPathBanner banner = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.Id == id) ??
                throw new NotFoundException("Путь к баннеру не найден");

            return banner.ToResponse();
        }

        public async Task<UserPathBannerResponse> GetByBoxIdAsync(Guid boxId)
        {
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId))
                throw new NotFoundException("Кейс не найден");

            UserPathBanner banner = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BoxId == boxId) ??
                throw new NotFoundException("Путь к баннеру не найден");

            return banner.ToResponse();
        }

        public async Task<UserPathBannerResponse> CreateAsync(UserPathBannerRequest request)
        {
            if (await _context.PathBanners.AnyAsync(upb => upb.UserId == request.UserId && upb.BoxId == request.BoxId))
                throw new ConflictException("Путь к баннеру уже используется");

            LootBox box = await _context.Boxes
                .Include(lb => lb.Inventories)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            LootBoxInventory inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.BoxId == request.BoxId && lbi.ItemId == request.ItemId) ??
                throw new NotFoundException("Предмет не найден");

            GameItem item = inventory.Item!;

            if (box.IsLocked)
                throw new ForbiddenException("Кейс не активен");
            if (!box.IsActiveBanner)
                throw new ForbiddenException("Баннер не активен");
            if (item.Cost <= box.Cost)
                throw new BadRequestException("Стоимость товара не может быть меньше стоимости кейса");

            UserPathBanner banner = request.ToEntity();

            banner.NumberSteps = (int)Math.Ceiling(item.Cost / (box.Cost * 0.2M));
            banner.FixedCost = item.Cost;

            if(banner.NumberSteps > 100)
                throw new BadRequestException("Стоимость предмета превышает стоимость кейса в 20 раз");

            await _context.PathBanners.AddAsync(banner);
            await _context.SaveChangesAsync();

            banner.Item = item;
            banner.Box = box;

            return banner.ToResponse();
        }

        public Task<UserPathBannerResponse> UpdateAsync(UserPathBannerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserPathBannerResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
