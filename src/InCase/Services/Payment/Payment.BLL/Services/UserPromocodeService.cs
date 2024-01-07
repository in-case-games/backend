using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
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

        public async Task<UserPromocode?> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default) => 
            await _context.UserPromocodes
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id && ur.UserId == userId, cancellation);

        public async Task CreateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default)
        {
            if (!await _context.UserPromocodes.AnyAsync(up => up.UserId == template.UserId, cancellation))
                throw new BadRequestException("Уже используется промокод");

            await _context.UserPromocodes.AddAsync(new UserPromocode
            {
                Id = template.Id,
                Discount = template.Discount,
                UserId = template.UserId,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default)
        {
            var entityOld = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId, cancellation) ??
                throw new NotFoundException("Промокод пользователя не найден");

            _context.Entry(entityOld).CurrentValues.SetValues(new UserPromocode
            {
                Id = template.Id,
                Discount = template.Discount,
                UserId = template.UserId,
            });
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
