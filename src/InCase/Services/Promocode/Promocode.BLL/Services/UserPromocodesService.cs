using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.MassTransit;
using Promocode.BLL.Models;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Services;
public class UserPromoCodesService(ApplicationDbContext context, BasePublisher publisher) : IUserPromoCodesService
{
    public async Task<UserPromoCodeResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var promoCode = await context.UserPromoCodes
            .Include(uhp => uhp.PromoCode)
            .Include(uhp => uhp.PromoCode!.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(uhp => uhp.Id == id, cancellation) ??
            throw new NotFoundException("История активации промокода не найдена");

        return promoCode.ToResponse();
    }

    public async Task<UserPromoCodeResponse> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        var promoCode = await context.UserPromoCodes
            .Include(uhp => uhp.PromoCode)
            .Include(uhp => uhp.PromoCode!.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(uhp => uhp.Id == id, cancellation) ??
            throw new NotFoundException("История активации промокода не найдена");

        return promoCode.UserId == userId ?
            promoCode.ToResponse() :
            throw new ForbiddenException("История активации числится на другого пользователя");
    }

    public async Task<List<UserPromoCodeResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

        var promoCode = await context.UserPromoCodes
            .Include(uhp => uhp.PromoCode)
            .Include(uhp => uhp.PromoCode!.Type)
            .AsNoTracking()
            .Where(uhp => uhp.UserId == userId)
            .OrderByDescending(uhp => uhp.Date)
            .Take(count)
            .ToListAsync(cancellation);

        return promoCode.ToResponse();
    }

    public async Task<List<UserPromoCodeResponse>> GetAsync(int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

        var history = await context.UserPromoCodes
            .Include(uhp => uhp.PromoCode)
            .Include(uhp => uhp.PromoCode!.Type)
            .AsNoTracking()
            .OrderByDescending(uhp => uhp.Date)
            .Take(count)
            .ToListAsync(cancellation);

        return history.ToResponse();
    }

    public async Task<UserPromoCodeResponse> ActivateAsync(Guid userId, string name, CancellationToken cancellation = default)
    {
        var promoCode = await context.PromoCodes
            .Include(p => p.Type)
            .FirstOrDefaultAsync(p => p.Name == name, cancellation) ??
            throw new NotFoundException("Промокод не найден");

        var isUsedType = await context.UserPromoCodes.AnyAsync(up =>
            up.PromoCode!.Type!.Id == promoCode.TypeId &&
            up.IsActivated == false &&
            up.UserId == userId, cancellation);

        if (promoCode.NumberActivations <= 0 || promoCode.ExpirationDate <= DateTime.UtcNow)
            throw new ForbiddenException("Промокод истёк");
        if (await context.UserPromoCodes.AnyAsync(uhp => uhp.PromoCodeId == promoCode.Id, cancellation))
            throw new ConflictException("Промокод уже используется");
        if (isUsedType)
            throw new ConflictException("Тип промокода уже используется");

        var userPromoCode = new UserPromoCode
        {
            IsActivated = false,
            Date = DateTime.UtcNow,
            PromoCodeId = promoCode.Id,
            UserId = userId,
            PromoCode = promoCode,
        };

        promoCode.NumberActivations--;

        await context.UserPromoCodes.AddAsync(userPromoCode, cancellation);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(userPromoCode.ToTemplate(), cancellation);

        return userPromoCode.ToResponse();
    }

    public async Task<UserPromoCodeResponse> ExchangeAsync(Guid userId, string name, CancellationToken cancellation = default)
    {
        var promoCode = await context.PromoCodes
            .Include(p => p.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name, cancellation) ??
            throw new NotFoundException("Промокод не найден");

        if (promoCode.NumberActivations <= 0 || promoCode.ExpirationDate <= DateTime.UtcNow)
            throw new ConflictException("Промокод истёк");
        if (await context.UserPromoCodes.AnyAsync(up => up.PromoCodeId == promoCode.Id, cancellation))
            throw new ConflictException("Промокод уже используется/использован");

        var userPromoCode = await context.UserPromoCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(uhp =>
            uhp.PromoCode!.Type!.Id == promoCode.TypeId &&
            uhp.IsActivated == false &&
            uhp.UserId == userId, cancellation) ??
            throw new ConflictException("Промокод не активировался");

        var promoCodeOld = await context.PromoCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == userPromoCode.PromoCodeId, cancellation) ??
            throw new NotFoundException("Прошлый промокод не найден");

        promoCodeOld.NumberActivations++;
        promoCode.NumberActivations--;

        userPromoCode.Date = DateTime.UtcNow;
        userPromoCode.PromoCodeId = promoCode.Id;

        context.Entry(promoCodeOld).Property(p => p.NumberActivations).IsModified = true;
        context.Entry(promoCode).Property(p => p.NumberActivations).IsModified = true;
        context.Entry(userPromoCode).Property(p => p.Date).IsModified = true;
        context.Entry(userPromoCode).Property(p => p.PromoCodeId).IsModified = true;
        await context.SaveChangesAsync(cancellation);

        userPromoCode.PromoCode = promoCode;

        await publisher.SendAsync(userPromoCode.ToTemplate(), cancellation);

        return userPromoCode.ToResponse();
    }
}