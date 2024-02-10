using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services;
public class UserPromoCodeService(ApplicationDbContext context) : IUserPromoCodeService
{
    public async Task<UserPromoCode?> GetAsync(Guid userId, CancellationToken cancellation = default) => 
        await context.UserPromoCodes
        .AsNoTracking()
        .FirstOrDefaultAsync(ur => ur.UserId == userId, cancellation);

    public async Task CreateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default)
    {
        if (await context.UserPromoCodes.AnyAsync(up => up.UserId == template.UserId, cancellation))
            throw new BadRequestException("Уже используется промокод");

        await context.UserPromoCodes.AddAsync(new UserPromoCode
        {
            Id = template.Id,
            Discount = template.Discount,
            UserId = template.UserId,
        }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default)
    {
        var entityOld = await context.UserPromoCodes
            .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId, cancellation) ??
            throw new NotFoundException("Промокод пользователя не найден");

        entityOld.Discount = template.Discount;
        await context.SaveChangesAsync(cancellation);
    }
}