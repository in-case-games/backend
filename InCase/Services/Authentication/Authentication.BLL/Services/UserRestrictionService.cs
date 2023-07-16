using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class UserRestrictionService : IUserRestrictionService
    {
        private readonly ApplicationDbContext _context;

        public UserRestrictionService(ApplicationDbContext context)
        {
            _context = context;            
        }
        public async Task<UserRestrictionResponse> GetAsync(Guid id)
        {
            UserRestriction restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Эффект не найден");

            return restriction.ToResponse();
        }

        public async Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request, bool isNewGuid = false)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");

            UserRestriction restriction = request.ToEntity(isNewGuid: isNewGuid);

            await _context.Restrictions.AddAsync(restriction);
            await _context.SaveChangesAsync();

            return restriction.ToResponse();
        }

        public async Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");

            UserRestriction restriction = await _context.Restrictions
                .FirstOrDefaultAsync(ur => ur.Id == request.Id) ??
                throw new NotFoundException("Эффект не найден");

            UserRestriction restrictionNew = request.ToEntity(isNewGuid: false);

            _context.Entry(restriction).CurrentValues.SetValues(restrictionNew);
            await _context.SaveChangesAsync();

            return restriction.ToResponse();
        }

        public async Task<UserRestrictionResponse> DeleteAsync(Guid id)
        {
            UserRestriction restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Эффект не найден");

            _context.Restrictions.Remove(restriction);
            await _context.SaveChangesAsync();

            return restriction.ToResponse();
        }
    }
}
