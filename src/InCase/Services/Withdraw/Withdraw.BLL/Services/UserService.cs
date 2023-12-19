using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new BadRequestException("Пользователь существует");

            await _context.Users.AddAsync(new User { Id = template.Id }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
