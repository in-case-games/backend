using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class UserService(ApplicationDbContext context) : IUserService
    {
        public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new NotFoundException("Пользователь существует");

            await context.Users.AddAsync(new User { Id = template.Id, }, cancellation);
            await context.AdditionalInfos.AddAsync(new UserAdditionalInfo
            {
                UserId = template.Id,
                Balance = 0,
                IsGuestMode = false,
            }, cancellation);
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
}
