using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new ForbiddenException("Пользователь существует");

            await _context.Users.AddAsync(new User
            {
                Id = template.Id,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id , cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
