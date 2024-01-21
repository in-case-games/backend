using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.Services;

public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default) => 
        await context.Users
        .Include(u => u.AdditionalInfo)
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetAsync(string email, CancellationToken cancellationToken = default) => 
        await context.Users
        .Include(u => u.AdditionalInfo)
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task CreateAsync(UserTemplate template, CancellationToken cancellationToken = default)
    {
        if (await context.Users.AnyAsync(u => u.Id == template.Id || u.Email == template.Email, cancellationToken))
            throw new ForbiddenException("Пользователь существует");

        await context.Users.AddAsync(new User
        {
            Id = template.Id,
            Email = template.Email,
        }, cancellationToken);
        await context.AdditionalInfos.AddAsync(new UserAdditionalInfo
        {
            IsNotifyEmail = true,
            UserId = template.Id,
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserTemplate template, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == template.Id, cancellationToken) ??
            throw new NotFoundException("Пользователь не найден");

        user.Email = template.Email;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken) ??
            throw new NotFoundException("Пользователь не найден");

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}