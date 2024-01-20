using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class UserPromocodeService(ApplicationDbContext context) : IUserPromocodeService
    {
        public async Task<UserPromocode?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await context.UserPromocodes
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation);

        public async Task CreateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default)
        {
            if (await context.UserPromocodes.AnyAsync(up => up.UserId == template.UserId, cancellation))
                throw new BadRequestException("Уже используется промокод");

            await context.UserPromocodes.AddAsync(new UserPromocode
            {
                Id = template.Id,
                Discount = template.Discount,
                UserId = template.UserId,
            }, cancellation);
            await context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default)
        {
            var entityOld = await context.UserPromocodes
                .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId, cancellation) ??
                throw new NotFoundException("Промокод пользователя не найден");

            entityOld.Discount = template.Discount;

            await context.SaveChangesAsync(cancellation);
        }
    }
}
