using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class UserPromocodeService : IUserPromocodeService
    {
        private readonly ApplicationDbContext _context;

        public UserPromocodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPromocodeResponse> GetAsync(Guid id)
        {
            UserPromocode userPromocode = await _context.UsersPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Промокод пользователя не найден");

            return userPromocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> CreateAsync(UserPromocodeRequest request, bool isNewGuid = false)
        {
            if (!await _context.UsersPromocodes.AnyAsync(up => up.UserId == request.UserId))
                throw new BadRequestException("Уже используется промокод");

            UserPromocode entity = request.ToEntity(isNewGuid: isNewGuid);

            await _context.UsersPromocodes.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ToResponse();
        }

        public async Task<UserPromocodeResponse> UpdateAsync(UserPromocodeRequest request)
        {
            UserPromocode entityOld = await _context.UsersPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == request.Id && ur.UserId == request.UserId) ??
                throw new NotFoundException("Промокод пользователя не найден");

            UserPromocode entity = request.ToEntity();

            _context.Entry(entityOld).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return entity.ToResponse();
        }
    }
}
