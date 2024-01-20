using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using Promocode.DAL.Data;

namespace Promocode.BLL.Services
{
    public class PromocodeService(ApplicationDbContext context) : IPromocodeService
    {
        public async Task<List<PromocodeResponse>> GetAsync(CancellationToken cancellation = default)
        {
            var promocodes = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return promocodes.ToResponse();
        }

        public async Task<PromocodeResponse> GetAsync(string name, CancellationToken cancellation = default)
        {
            var promocode = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Name == name, cancellation) ??
                throw new NotFoundException("Промокод не найден");

            return promocode.ToResponse();
        }

        public async Task<List<PromocodeResponse>> GetEmptyPromocodesAsync(CancellationToken cancellation = default)
        {
            var promocodes = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .Where(pe => pe.NumberActivations <= 0)
                .ToListAsync(cancellation);

            return promocodes.ToResponse();
        }

        public async Task<List<PromocodeTypeResponse>> GetTypesAsync(CancellationToken cancellation = default)
        {
            var types = await context.PromocodesTypes
                .AsNoTracking()
                .ToListAsync(cancellation);

            return types.ToResponse();
        }

        public async Task<PromocodeResponse> CreateAsync(PromocodeRequest request, CancellationToken cancellation = default)
        {
            ValidationService.IsPromocode(request);

            if (await context.Promocodes.AnyAsync(pe => pe.Name == request.Name, cancellation))
                throw new ConflictException("Имя промокода уже используется");

            var type = await context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId, cancellation) ?? 
                throw new NotFoundException("Тип промокода не найден");

            var entity = request.ToEntity(true);

            await context.Promocodes.AddAsync(entity, cancellation);
            await context.SaveChangesAsync(cancellation);

            entity.Type = type;

            return entity.ToResponse();
        }

        public async Task<PromocodeResponse> UpdateAsync(PromocodeRequest request, CancellationToken cancellation = default)
        {
            ValidationService.IsPromocode(request);

            var isExist = await context.Promocodes
                .AsNoTracking()
                .AnyAsync(pe => pe.Name == request.Name && pe.Id != request.Id, cancellation);

            if (isExist) throw new ConflictException("Имя промокода уже занято");
            if (!await context.Promocodes.AnyAsync(pe => pe.Id == request.Id, cancellation))
                throw new NotFoundException("Промокод не найден");

            var type = await context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId, cancellation) ??
                throw new NotFoundException("Тип промокода не найден");

            var promocode = request.ToEntity();

            context.Promocodes.Update(promocode);
            await context.SaveChangesAsync(cancellation);

            promocode.Type = type;

            return promocode.ToResponse();
        }

        public async Task<PromocodeResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var promocode = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Id == id, cancellation) ??
                throw new NotFoundException("Промокод не найден");

            context.Promocodes.Remove(promocode);
            await context.SaveChangesAsync(cancellation);

            return promocode.ToResponse();
        }
    }
}
