using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
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

        public async Task<UserResponse> GetAsync(Guid id)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
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
                .Include(u => u.Restrictions)
                .Include(u => u.OwnerRestrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateLoginAsync(UserRequest request)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.Restrictions)
                .Include(u => u.OwnerRestrictions)
                .FirstOrDefaultAsync(u => u.Id == request.Id) ??
                throw new NotFoundException("Пользователь не найден");

            user.Login = request.Login;

            await _context.SaveChangesAsync();

            return user.ToResponse();
        }
    }
}
