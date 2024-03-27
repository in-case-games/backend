using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services;
public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
    {
        if (await context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
            throw new ForbiddenException("Пользователь существует");

        await context.Users.AddAsync(new User
        {
            Id = template.Id,
        }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id , cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellation);
    }
}