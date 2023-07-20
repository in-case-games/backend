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

        public async Task<User?> GetAsync(Guid id) => await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        public async Task CreateAsync(UserTemplate template)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id))
                throw new ForbiddenException("Пользователь существует");

            User user = template.ToEntity();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
