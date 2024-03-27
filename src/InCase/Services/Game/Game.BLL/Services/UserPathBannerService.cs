using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services;
public class UserPathBannerService(ApplicationDbContext context) : IUserPathBannerService
{
    public async Task<List<UserPathBannerResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var banners = await context.UserPathBanners
            .Include(upb => upb.Item)
            .Include(upb => upb.Box)
            .AsNoTracking()
            .Where(upb => upb.UserId == userId)
            .ToListAsync(cancellation);

        return banners.ToResponse();
    }

    public async Task<List<UserPathBannerResponse>> GetByItemIdAsync(Guid itemId, Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");
        if (!await context.GameItems.AnyAsync(gi => gi.Id == itemId, cancellation))
            throw new NotFoundException("Предмет не найден");

        var banners = await context.UserPathBanners
            .Include(upb => upb.Item)
            .Include(upb => upb.Box)
            .AsNoTracking()
            .Where(upb => upb.ItemId == itemId && upb.UserId == userId)
            .ToListAsync(cancellation);

        return banners.ToResponse();
    }

    public async Task<UserPathBannerResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var banner = await context.UserPathBanners
            .Include(upb => upb.Item)
            .Include(upb => upb.Box)
            .AsNoTracking()
            .FirstOrDefaultAsync(upb => upb.Id == id && upb.UserId == userId, cancellation) ??
            throw new NotFoundException("Путь к баннеру не найден");

        return banner.ToResponse();
    }

    public async Task<UserPathBannerResponse> GetByBoxIdAsync(Guid boxId, Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");
        if (!await context.LootBoxes.AnyAsync(lb => lb.Id == boxId, cancellation))
            throw new NotFoundException("Кейс не найден");

        var banner = await context.UserPathBanners
            .Include(upb => upb.Item)
            .Include(upb => upb.Box)
            .AsNoTracking()
            .FirstOrDefaultAsync(upb => upb.BoxId == boxId && upb.UserId == userId, cancellation) ??
            throw new NotFoundException("Путь к баннеру не найден");

        return banner.ToResponse();
    }

    public async Task<UserPathBannerResponse> CreateAsync(UserPathBannerRequest request, CancellationToken cancellation = default)
    {
        if (!await context.UserAdditionalInfos.AnyAsync(uai => uai.UserId == request.UserId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        if (await context.UserPathBanners
            .AnyAsync(upb => upb.UserId == request.UserId && upb.BoxId == request.BoxId, cancellation))
            throw new ConflictException("Путь к баннеру уже используется");

        var box = await context.LootBoxes
            .Include(lb => lb.Inventories)
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == request.BoxId, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        var inventory = await context.LootBoxInventories
            .Include(lbi => lbi.Item)
            .AsNoTracking()
            .FirstOrDefaultAsync(lbi => lbi.BoxId == request.BoxId && 
            lbi.ItemId == request.ItemId, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        if (box.IsLocked)
            throw new ForbiddenException("Кейс не активен");
        if (box.ExpirationBannerDate is null || box.ExpirationBannerDate < DateTime.UtcNow)
            throw new ForbiddenException("Баннер не активен");
        if (inventory.Item!.Cost <= box.Cost)
            throw new BadRequestException("Стоимость товара не может быть меньше стоимости кейса");

        var banner = new UserPathBanner
        {
            BoxId = request.BoxId,
            ItemId = request.ItemId,
            UserId = request.UserId,
            NumberSteps = (int)Math.Ceiling(inventory.Item.Cost / (box.Cost * 0.2M)),
            FixedCost = inventory.Item.Cost
        };

        if (banner.NumberSteps > 100)
            throw new BadRequestException("Стоимость предмета превышает стоимость кейса в 20 раз");

        await context.UserPathBanners.AddAsync(banner, cancellation);
        await context.SaveChangesAsync(cancellation);

        banner.Item = inventory.Item;
        banner.Box = box;

        return banner.ToResponse();
    }

    public async Task<UserPathBannerResponse> UpdateAsync(UserPathBannerRequest request, CancellationToken cancellation = default)
    {
        if (!await context.UserAdditionalInfos.AnyAsync(uai => uai.UserId == request.UserId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var banner = await context.UserPathBanners
            .Include(usp => usp.Box)
            .Include(usp => usp.Item)
            .FirstOrDefaultAsync(usp => usp.Id == request.Id, cancellation) ??
            throw new NotFoundException("Путь к баннеру не найден");

        if (banner.UserId != request.UserId) throw new ForbiddenException("Нельзя поменять пользователя");
        if (banner.BoxId != request.BoxId) throw new ForbiddenException("Нельзя поменять кейс");
        if (banner.ItemId == request.ItemId) throw new BadRequestException("Предмет уже используется");

        var item = await context.GameItems
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == request.ItemId, cancellation) ?? 
            throw new NotFoundException("Предмет не найден");

        if (item.Cost <= banner.Box!.Cost)
            throw new BadRequestException("Стоимость товара не может быть меньше стоимости кейса");

        banner.NumberSteps = (int)Math.Ceiling(item.Cost / (banner.Box.Cost * 0.2M));
        banner.FixedCost = item.Cost;
        banner.ItemId = request.ItemId;

        if (banner.NumberSteps > 100)
            throw new BadRequestException("Стоимость предмета превышает стоимость кейса в 20 раз");

        context.UserPathBanners.Update(banner);
        await context.SaveChangesAsync(cancellation);

        banner.Item = item;

        return banner.ToResponse();
    }

    public async Task<UserPathBannerResponse> DeleteAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        if(!await context.UserAdditionalInfos.AnyAsync(uai => uai.UserId == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");
        
        var banner = await context.UserPathBanners
            .Include(usp => usp.Box)
            .Include(usp => usp.Item)
            .AsNoTracking()
            .FirstOrDefaultAsync(usp => usp.Id == id && usp.UserId == userId, cancellation) ??
            throw new NotFoundException("Путь к баннеру не найден");

        context.UserPathBanners.Remove(banner);
        await context.SaveChangesAsync(cancellation);

        return banner.ToResponse();
    }
}