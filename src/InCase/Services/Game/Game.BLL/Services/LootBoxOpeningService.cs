using Game.BLL.Constants;
using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Game.BLL.Services;
public class LootBoxOpeningService(
	ApplicationDbContext context,
	ILogger<LootBoxOpeningService> logger, 
	IBasePublisher publisher,
	IApplicationDbContextWrapper contextWrapper) : ILootBoxOpeningService
{
	public async Task<GameItemResponse> OpenBoxAsync(Guid userId, Guid id, CancellationToken cancellation = default)
	{
		var box = await context.LootBoxes
			.Include(lb => lb.Inventories!)
			.ThenInclude(lbi => lbi.Item)
			.AsNoTracking()
			.FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
			throw new NotFoundException("Кейс не найден");

		var info = await context.UserAdditionalInfos
			.AsNoTracking()
			.FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
			throw new NotFoundException("Пользователь не найден");

		var promo = await context.UserPromoCodes
			.AsNoTracking()
			.FirstOrDefaultAsync(uhp => uhp.UserId == userId, cancellation);

		if (box.IsLocked) throw new ForbiddenException("Кейс заблокирован");
		if (info.Balance < box.Cost) throw new PaymentRequiredException("Недостаточно средств");
		if (promo is not null)
		{
			await publisher.SendAsync(new UserPromoCodeBackTemplate { Id = promo.Id }, cancellation);

			context.UserPromoCodes.Remove(promo);
		}

		var path = await context.UserPathBanners
			.AsNoTracking()
			.FirstOrDefaultAsync(upb => upb.BoxId == box.Id && upb.UserId == userId, cancellation);

		var discount = promo?.Discount ?? 0;
		var boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

		info.Balance -= boxCost;
		box.Balance += boxCost;

		var isPlayBanner = path is not null && 
			box.ExpirationBannerDate is not null && 
			box.ExpirationBannerDate >= DateTime.UtcNow;
		var winItem = OpenLootBoxService.RandomizeBySmallest(in box, isPlayBanner);
		var revenue = boxCost * CommonConstants.RevenuePercentage;
		var expenses = winItem.Cost + revenue;

		if (isPlayBanner)
		{
			--path!.NumberSteps;

			var retentionBanner = box.Cost * CommonConstants.RetentionPercentageBanner;

			revenue = boxCost * CommonConstants.RevenuePercentageBanner;
			expenses = retentionBanner + revenue;

			if (path.NumberSteps == 0)
			{
				winItem = box.Inventories?.FirstOrDefault(f => f.ItemId == path.ItemId)?.Item ??
					await context.GameItems
						.AsNoTracking()
						.FirstOrDefaultAsync(i => i.Id == path.ItemId, cancellation);
				winItem!.Cost = path.FixedCost;

				context.UserPathBanners.Remove(path);
			}
			else
			{
				expenses += winItem.Cost;
				context.UserPathBanners.Attach(path);
				contextWrapper.SetEntryIsModifyProperty(path, p => p.NumberSteps);
			}
		}

		var opening = new UserOpening
		{
			UserId = userId,
			BoxId = box.Id,
			ItemId = winItem.Id,
			Date = DateTime.UtcNow
		};

		box.Balance -= expenses;

		logger.LogTrace($"ID={id}|UID={userId}|WID={winItem.Id}|COST={boxCost}|REV={revenue}|EXP={expenses}");

		contextWrapper.SetEntryIsModifyProperty(info, p => p.Balance);
		contextWrapper.SetEntryIsModifyProperty(box, p => p.Balance);

		await context.UserOpenings.AddAsync(opening, cancellation);
		await context.SaveChangesAsync(cancellation);

		logger.LogTrace($"ID={id}|Box_Balance={box.Balance}|Зафиксировано");

		await publisher.SendAsync(new UserInventoryTemplate
		{
			Date = DateTime.UtcNow,
			FixedCost = winItem.Cost,
			ItemId = winItem.Id,
			UserId = userId,
		}, cancellation);
		await publisher.SendAsync(new SiteStatisticsTemplate { LootBoxes = 1 }, cancellation);
		await publisher.SendAsync(new SiteStatisticsAdminTemplate
		{
			RevenueLootBoxCommission = revenue,
			FundsUsersInventories = winItem.Cost
		}, cancellation);

		return new GameItemResponse
		{
			Id = winItem.Id,
			Cost = winItem.Cost,
		};
	}

	public async Task<GameItemResponse> OpenVirtualBoxAsync(Guid userId, Guid id, CancellationToken cancellation = default)
	{
		var userInfo = await context.UserAdditionalInfos
			.AsNoTracking()
			.FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
			throw new NotFoundException("Пользователь не найден");

		var box = await context.LootBoxes
			.Include(lb => lb.Inventories!)
				.ThenInclude(lbi => lbi.Item)
			.AsNoTracking()
			.FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
			throw new NotFoundException("Кейс не найден");

		if (!userInfo.IsGuestMode) throw new ForbiddenException("Не включен режим гостя");

		box.VirtualBalance += box.Cost;

		var winItem = OpenLootBoxService.RandomizeBySmallest(in box, false, true);
		var revenue = box.Cost * CommonConstants.RevenuePercentage;
		var expenses = winItem.Cost + revenue;

		box.VirtualBalance -= expenses;

		context.LootBoxes.Attach(box);
		contextWrapper.SetEntryIsModifyProperty(box, p => p.VirtualBalance);
		await context.SaveChangesAsync(cancellation);

		return new GameItemResponse
		{
			Id = winItem.Id,
			Cost = winItem.Cost,
		};
	}

	public async Task<List<GameItemBigOpenResponse>> OpenVirtualBoxAsync(
		Guid userId,
		Guid id, 
		int count, 
		bool isAdmin = false,
		CancellationToken cancellation = default)
	{
		if (count < 1 || (count > 100 && !isAdmin))
			throw new BadRequestException("Количество открытий должно быть в диапазоне от 1 до 100");

		var userInfo = await context.UserAdditionalInfos
			.AsNoTracking()
			.FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
			throw new NotFoundException("Пользователь не найден");

		var box = await context.LootBoxes
			.Include(lb => lb.Inventories!)
				.ThenInclude(lbi => lbi.Item)
			.AsNoTracking()
			.FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
			throw new NotFoundException("Кейс не найден");

		if (!userInfo.IsGuestMode) throw new ForbiddenException("Не включен режим гостя");

		context.LootBoxes.Attach(box);
		contextWrapper.SetEntryIsModifyProperty(box, p => p.VirtualBalance);

		var winItems = new List<GameItemBigOpenResponse>();

		for (var i = 0; i < count; i++)
		{
			try
			{
				box.VirtualBalance += box.Cost;

				var winItem = OpenLootBoxService.RandomizeBySmallest(in box, false, true);
				var revenue = box.Cost * CommonConstants.RevenuePercentage;
				var expenses = winItem.Cost + revenue;

				box.VirtualBalance -= expenses;

				var index = winItems.FindIndex(gi => gi.Id == winItem.Id);

				if (index != -1) winItems[index].Count++;
				else winItems.Add(new GameItemBigOpenResponse 
				{ 
					Id = winItem.Id, 
					Cost = winItem.Cost, 
					Count = 1 
				});

				await context.SaveChangesAsync(cancellation);
			}
			catch(Exception ex)
			{
				await context.SaveChangesAsync(cancellation);

				throw new StatusCodeExtendedException(ErrorCodes.UnknownError, ex.Message, winItems);
			}
		}

		return winItems;
	}
}