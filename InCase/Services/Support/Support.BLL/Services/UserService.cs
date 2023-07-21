using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.DAL.Data;
using Support.DAL.Entities;

namespace Support.BLL.Services
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
