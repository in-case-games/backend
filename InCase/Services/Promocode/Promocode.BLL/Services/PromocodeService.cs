using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;
using System.Threading;

namespace Promocode.BLL.Services
{
    public class PromocodeService : IPromocodeService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public PromocodeService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<PromocodeResponse>> GetAsync()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<PromocodeEntity> promocodes = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .ToListAsync();

            return promocodes.ToResponse();
        }

        public async Task<PromocodeResponse> GetAsync(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeEntity promocode = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            return promocode.ToResponse();
        }

        public async Task<List<PromocodeResponse>> GetEmptyPromocodesAsync()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<PromocodeEntity> promocodes = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .Where(pe => pe.NumberActivations <= 0)
                .ToListAsync();

            return promocodes.ToResponse();
        }

        public async Task<List<PromocodeTypeResponse>> GetTypesAsync()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<PromocodeType> types = await context.PromocodesTypes
                .AsNoTracking()
                .ToListAsync();

            return types.ToResponse();
        }

        public async Task<PromocodeResponse> CreateAsync(PromocodeRequest request)
        {
            ValidationService.IsPromocode(request);

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType type = await context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId) ?? 
                throw new NotFoundException("Тип промокода не найден");

            if (await context.Promocodes.AnyAsync(pe => pe.Name == request.Name))
                throw new ConflictException("Имя промокода уже используется");

            PromocodeEntity entity = request.ToEntity(true);

            await context.Promocodes.AddAsync(entity);
            await context.SaveChangesAsync();

            entity.Type = type;

            return entity.ToResponse();
        }

        public async Task<PromocodeResponse> UpdateAsync(PromocodeRequest request)
        {
            ValidationService.IsPromocode(request);

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType type = await context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId) ??
                throw new NotFoundException("Тип промокода не найден");

            bool isExist = await context.Promocodes
                .AsNoTracking()
                .AnyAsync(pe => pe.Name == request.Name && pe.Id != request.Id);

            if (isExist)
                throw new ConflictException("Имя промокода уже занято");
            if (!await context.Promocodes.AnyAsync(pe => pe.Id == request.Id))
                throw new NotFoundException("Промокод не найден");

            PromocodeEntity promocode = request.ToEntity();

            context.Promocodes.Update(promocode);
            await context.SaveChangesAsync();

            promocode.Type = type;

            return promocode.ToResponse();
        }

        public async Task<PromocodeResponse> DeleteAsync(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeEntity promocode = await context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Id == id) ??
                throw new NotFoundException("Промокод не найден");

            context.Promocodes.Remove(promocode);
            await context.SaveChangesAsync();

            return promocode.ToResponse();
        }
    }
}
