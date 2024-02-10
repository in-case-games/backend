using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Interfaces;
using Support.DAL.Data;
using Support.DAL.Entities;

namespace Support.BLL.Services;
public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => 
        await context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == id, cancellation);

    public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
    {
        if (await context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
            throw new ForbiddenException("Пользователь существует");

        await context.Users.AddAsync(new User { Id = template.Id }, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellation);
    }
}