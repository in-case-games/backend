using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
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
            UserPromocode userPromocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Промокод пользователя не найден");

            return userPromocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> CreateAsync(UserPromocodeRequest request, bool isNewGuid = false)
        {
            if (!await _context.UserPromocodes.AnyAsync(up => up.UserId == request.UserId))
                throw new BadRequestException("Уже используется промокод");

            UserPromocode entity = request.ToEntity(isNewGuid: isNewGuid);

            await _context.UserPromocodes.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ToResponse();
        }

        public async Task<UserPromocodeResponse> UpdateAsync(UserPromocodeRequest request)
        {
            UserPromocode entityOld = await _context.UserPromocodes
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
