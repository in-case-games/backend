using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
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

        public async Task<UserPromocode?> GetAsync(Guid id, Guid userId) => await _context.UserPromocodes
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id);

        public async Task CreateAsync(UserPromocodeTemplate template)
        {
            if (await _context.UserPromocodes.AnyAsync(up => up.UserId == template.UserId))
                throw new BadRequestException("Уже используется промокод");

            UserPromocode entity = template.ToEntity();

            await _context.UserPromocodes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserPromocodeTemplate template)
        {
            UserPromocode entityOld = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId) ??
                throw new NotFoundException("Промокод пользователя не найден");

            UserPromocode entity = template.ToEntity();

            _context.Entry(entityOld).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
