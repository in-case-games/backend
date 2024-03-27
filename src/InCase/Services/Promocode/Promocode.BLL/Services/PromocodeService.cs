using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using Promocode.DAL.Data;

namespace Promocode.BLL.Services;
public class PromoCodeService(ApplicationDbContext context) : IPromoCodeService
{
    public async Task<List<PromoCodeResponse>> GetAsync(CancellationToken cancellation = default)
    {
        var promoCodes = await context.PromoCodes
            .Include(pe => pe.Type)
            .AsNoTracking()
            .ToListAsync(cancellation);

        return promoCodes.ToResponse();
    }

    public async Task<PromoCodeResponse> GetAsync(string name, CancellationToken cancellation = default)
    {
        var promoCode = await context.PromoCodes
            .Include(pe => pe.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(pe => pe.Name == name, cancellation) ??
            throw new NotFoundException("Промокод не найден");

        return promoCode.ToResponse();
    }

    public async Task<List<PromoCodeResponse>> GetEmptyPromoCodesAsync(CancellationToken cancellation = default)
    {
        var promoCodes = await context.PromoCodes
            .Include(pe => pe.Type)
            .AsNoTracking()
            .Where(pe => pe.NumberActivations <= 0)
            .ToListAsync(cancellation);

        return promoCodes.ToResponse();
    }

    public async Task<List<PromoCodeTypeResponse>> GetTypesAsync(CancellationToken cancellation = default)
    {
        var types = await context.PromoCodesTypes
            .AsNoTracking()
            .ToListAsync(cancellation);

        return types.ToResponse();
    }

    public async Task<PromoCodeResponse> CreateAsync(PromoCodeRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsPromoCode(request);

        if (await context.PromoCodes.AnyAsync(pe => pe.Name == request.Name, cancellation))
            throw new ConflictException("Имя промокода уже используется");

        var type = await context.PromoCodesTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == request.TypeId, cancellation) ?? 
            throw new NotFoundException("Тип промокода не найден");

        var entity = request.ToEntity(true);

        await context.PromoCodes.AddAsync(entity, cancellation);
        await context.SaveChangesAsync(cancellation);

        entity.Type = type;

        return entity.ToResponse();
    }

    public async Task<PromoCodeResponse> UpdateAsync(PromoCodeRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsPromoCode(request);

        var isExist = await context.PromoCodes
            .AsNoTracking()
            .AnyAsync(pe => pe.Name == request.Name && pe.Id != request.Id, cancellation);

        if (isExist) throw new ConflictException("Имя промокода уже занято");
        if (!await context.PromoCodes.AnyAsync(pe => pe.Id == request.Id, cancellation))
            throw new NotFoundException("Промокод не найден");

        var type = await context.PromoCodesTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == request.TypeId, cancellation) ??
            throw new NotFoundException("Тип промокода не найден");

        var promoCode = request.ToEntity();

        context.PromoCodes.Update(promoCode);
        await context.SaveChangesAsync(cancellation);

        promoCode.Type = type;

        return promoCode.ToResponse();
    }

    public async Task<PromoCodeResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var promoCode = await context.PromoCodes
            .Include(pe => pe.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(pe => pe.Id == id, cancellation) ??
            throw new NotFoundException("Промокод не найден");

        context.PromoCodes.Remove(promoCode);
        await context.SaveChangesAsync(cancellation);

        return promoCode.ToResponse();
    }
}