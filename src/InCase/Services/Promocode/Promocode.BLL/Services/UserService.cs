using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new ForbiddenException("Пользователь существует");

            User user = template.ToEntity();

            await _context.Users.AddAsync(user, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
