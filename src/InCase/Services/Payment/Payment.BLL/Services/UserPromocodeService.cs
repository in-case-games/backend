using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services;

public class UserPromocodeService(ApplicationDbContext context) : IUserPromocodeService
{
    public async Task<UserPromocode?> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default) => 
        await context.UserPromocodes
        .AsNoTracking()
        .FirstOrDefaultAsync(ur => ur.Id == id && ur.UserId == userId, cancellation);

    public async Task CreateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default)
    {
        if (!await context.UserPromocodes.AnyAsync(up => up.UserId == template.UserId, cancellation))
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
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId, cancellation) ??
            throw new NotFoundException("Промокод пользователя не найден");

        context.Entry(entityOld).CurrentValues.SetValues(new UserPromocode
        {
            Id = template.Id,
            Discount = template.Discount,
            UserId = template.UserId,
        });
        await context.SaveChangesAsync(cancellation);
    }
}