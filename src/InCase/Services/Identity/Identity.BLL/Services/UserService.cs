using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.MassTransit;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByConsumerAsync(Guid id) => await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
           
        public async Task<UserResponse> GetAsync(Guid id)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .Include(u => u.Restrictions)
                .Include(u => u.OwnerRestrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new NotFoundException("Пользователь не найден");

            return user.ToResponse();
        }

        public async Task<UserResponse> GetAsync(string login)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .Include(u => u.Restrictions)
                .Include(u => u.OwnerRestrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            return user.ToResponse();
        }

        public async Task CreateAsync(UserTemplate template)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id))
                throw new ForbiddenException("Пользователь существует");

            User user = template.ToEntity();
            UserRole role = await _context.Roles.FirstAsync(ur => ur.Name == "user");
            UserAdditionalInfo info = new()
            {
                UserId = template.Id,
                CreationDate = DateTime.UtcNow,
                RoleId = role.Id
            };

            await _context.Users.AddAsync(user);
            await _context.AdditionalInfos.AddAsync(info);
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

        public async Task UpdateLoginAsync(UserTemplate template)
        {
            User user = await _context.Users
                 .Include(u => u.AdditionalInfo)
                 .Include(u => u.Restrictions)
                 .Include(u => u.OwnerRestrictions)
                 .FirstOrDefaultAsync(u => u.Id == template.Id) ??
                 throw new NotFoundException("Пользователь не найден");

            user.Login = template.Login;

            await _context.SaveChangesAsync();
        }
    }
}
