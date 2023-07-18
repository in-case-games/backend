using Infrastructure.MassTransit.User;
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

        public async Task CreateAsync(UserPromocodeTemplate template)
        {
            if (!await _context.UsersPromocodes.AnyAsync(up => up.UserId == template.UserId))
                throw new BadRequestException("Уже используется промокод");

            UserPromocode entity = template.ToEntity();

            await _context.UsersPromocodes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserPromocodeTemplate template)
        {
            UserPromocode entityOld = await _context.UsersPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId) ??
                throw new NotFoundException("Промокод пользователя не найден");

            UserPromocode entity = template.ToEntity();

            _context.Entry(entityOld).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
