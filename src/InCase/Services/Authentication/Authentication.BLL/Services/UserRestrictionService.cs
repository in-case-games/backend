using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class UserRestrictionService : IUserRestrictionService
    {
        private readonly ApplicationDbContext _context;

        public UserRestrictionService(ApplicationDbContext context)
        {
            _context = context;            
        }
        public async Task<UserRestriction?> GetAsync(Guid id, CancellationToken cancellationToken = default) => await _context.Restrictions
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken);

        public async Task CreateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == template.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            UserRestriction restriction = template.ToEntity();

            await _context.Restrictions.AddAsync(restriction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == template.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            UserRestriction restriction = await _context.Restrictions
                .FirstOrDefaultAsync(ur => ur.Id == template.Id, cancellationToken) ??
                throw new NotFoundException("Эффект не найден");

            UserRestriction restrictionNew = template.ToEntity();

            _context.Entry(restriction).CurrentValues.SetValues(restrictionNew);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            UserRestriction restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ??
                throw new NotFoundException("Эффект не найден");

            _context.Restrictions.Remove(restriction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
