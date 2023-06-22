using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<UserPathBannerResponse>> GetByItemIdAsync(Guid itemId, Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.GameItems.AnyAsync(gi => gi.Id == itemId))
                throw new NotFoundException("Предмет не найден");

            List<UserPathBanner> banners = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .Where(upb => upb.ItemId == itemId && upb.UserId == userId)
                .ToListAsync();

            return banners.ToResponse();
        }

        public async Task<UserPathBannerResponse> GetByIdAsync(Guid id, Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            UserPathBanner banner = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.Id == id && upb.UserId == userId) ??
                throw new NotFoundException("Путь к баннеру не найден");

            return banner.ToResponse();
        }

        public async Task<UserPathBannerResponse> GetByBoxIdAsync(Guid boxId, Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId))
                throw new NotFoundException("Кейс не найден");

            UserPathBanner banner = await _context.PathBanners
                .Include(upb => upb.Item)
                .Include(upb => upb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BoxId == boxId && upb.UserId == userId) ??
                throw new NotFoundException("Путь к баннеру не найден");

            return banner.ToResponse();
        }

        public async Task<UserPathBannerResponse> CreateAsync(UserPathBannerRequest request)
        {
            if (!await _context.AdditionalInfos.AnyAsync(uai => uai.UserId == request.UserId))
                throw new NotFoundException("Пользователь не найден");

            if (await _context.PathBanners
                .AnyAsync(upb => upb.UserId == request.UserId && upb.BoxId == request.BoxId))
                throw new ConflictException("Путь к баннеру уже используется");

            LootBox box = await _context.Boxes
                .Include(lb => lb.Inventories)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == request.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            LootBoxInventory inventory = await _context.BoxInventories
                .Include(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.BoxId == request.BoxId && 
                lbi.ItemId == request.ItemId) ??
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

        public async Task<UserPathBannerResponse> UpdateAsync(UserPathBannerRequest request)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == request.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            UserPathBanner banner = await _context.PathBanners
                .Include(usp => usp.Box)
                .Include(usp => usp.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(usp => usp.Id == request.Id) ??
                throw new NotFoundException("Путь к баннеру не найден");

            if (banner.UserId != request.UserId)
                throw new ForbiddenException("Нельзя поменять пользователя");
            if (banner.BoxId != request.BoxId)
                throw new ForbiddenException("Нельзя поменять кейс");
            
            GameItem item = await _context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == request.ItemId) ?? 
                throw new NotFoundException("Предмет не найден");
            
            decimal totalSpent = banner.NumberSteps * banner.Box!.Cost * 0.2M;

            // TODO Notify rabbit mq
            // statistics.BalanceWithdrawn += totalSpent * 0.1M;

            info.Balance += totalSpent * 0.9M;

            _context.PathBanners.Update(banner);
            await _context.SaveChangesAsync();

            banner.Item = item;

            return banner.ToResponse();
        }

        public async Task<UserPathBannerResponse> DeleteAsync(Guid id, Guid userId)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");
            
            UserPathBanner banner = await _context.PathBanners
                .Include(usp => usp.Box)
                .Include(usp => usp.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(usp => usp.Id == id && usp.UserId == userId) ??
                throw new NotFoundException("Путь к баннеру не найден");

            decimal totalSpent = banner.NumberSteps * banner.Box!.Cost * 0.2M;

            // TODO Notify rabbit mq
            // statistics.BalanceWithdrawn += totalSpent * 0.1M;

            info.Balance += totalSpent * 0.9M;

            _context.PathBanners.Remove(banner);
            await _context.SaveChangesAsync();

            return banner.ToResponse();
        }
    }
}
