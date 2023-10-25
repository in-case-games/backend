using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Services
{
    public class PromocodeService : IPromocodeService
    {
        private readonly ApplicationDbContext _context;

        public PromocodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PromocodeResponse>> GetAsync()
        {
            List<PromocodeEntity> promocodes = await _context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .ToListAsync();

            return promocodes.ToResponse();
        }

        public async Task<PromocodeResponse> GetAsync(string name)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            return promocode.ToResponse();
        }

        public async Task<List<PromocodeResponse>> GetEmptyPromocodesAsync()
        {
            List<PromocodeEntity> promocodes = await _context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .Where(pe => pe.NumberActivations <= 0)
                .ToListAsync();

            return promocodes.ToResponse();
        }

        public async Task<List<PromocodeTypeResponse>> GetTypesAsync()
        {
            List<PromocodeType> types = await _context.PromocodesTypes
                .AsNoTracking()
                .ToListAsync();

            return types.ToResponse();
        }

        public async Task<PromocodeResponse> CreateAsync(PromocodeRequest request)
        {
            ValidationService.IsPromocode(request);

            if (await _context.Promocodes.AnyAsync(pe => pe.Name == request.Name))
                throw new ConflictException("Имя промокода уже используется");

            PromocodeType type = await _context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId) ?? 
                throw new NotFoundException("Тип промокода не найден");

            PromocodeEntity entity = request.ToEntity(true);

            await _context.Promocodes.AddAsync(entity);
            await _context.SaveChangesAsync();

            entity.Type = type;

            return entity.ToResponse();
        }

        public async Task<PromocodeResponse> UpdateAsync(PromocodeRequest request)
        {
            ValidationService.IsPromocode(request);

            bool isExist = await _context.Promocodes
                .AsNoTracking()
                .AnyAsync(pe => pe.Name == request.Name && pe.Id != request.Id);

            if (isExist)
                throw new ConflictException("Имя промокода уже занято");
            if (!await _context.Promocodes.AnyAsync(pe => pe.Id == request.Id))
                throw new NotFoundException("Промокод не найден");

            PromocodeType type = await _context.PromocodesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == request.TypeId) ??
                throw new NotFoundException("Тип промокода не найден");

            PromocodeEntity promocode = request.ToEntity();

            _context.Promocodes.Update(promocode);
            await _context.SaveChangesAsync();

            promocode.Type = type;

            return promocode.ToResponse();
        }

        public async Task<PromocodeResponse> DeleteAsync(Guid id)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(pe => pe.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Id == id) ??
                throw new NotFoundException("Промокод не найден");

            _context.Promocodes.Remove(promocode);
            await _context.SaveChangesAsync();

            return promocode.ToResponse();
        }
    }
}
