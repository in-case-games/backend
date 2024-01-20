using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class UserRestrictionService(ApplicationDbContext context) : IUserRestrictionService
    {
        public async Task<UserRestriction?> GetAsync(Guid id, CancellationToken cancellationToken = default) => 
            await context.Restrictions
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken);

        public async Task CreateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default)
        {
            if (!await context.Users.AnyAsync(u => u.Id == template.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            await context.Restrictions.AddAsync(new UserRestriction()
            {
                Id = template.Id,
                ExpirationDate = template.ExpirationDate,
                UserId = template.UserId
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default)
        {
            if (!await context.Users.AnyAsync(u => u.Id == template.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            var old = await context.Restrictions
                .FirstOrDefaultAsync(ur => ur.Id == template.Id, cancellationToken) ??
                throw new NotFoundException("Эффект не найден");;

            context.Entry(old).CurrentValues.SetValues(new UserRestriction()
            {
                Id = template.Id,
                ExpirationDate = template.ExpirationDate,
                UserId = template.UserId
            });
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var restriction = await context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ??
                throw new NotFoundException("Эффект не найден");

            context.Restrictions.Remove(restriction);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
