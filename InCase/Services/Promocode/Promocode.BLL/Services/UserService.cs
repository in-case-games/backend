using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
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

        public async Task<UserResponse> GetAsync(Guid id)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new NotFoundException("Пользователь не найден");

            return user.ToResponse();
        }

        public async Task<UserResponse> CreateAsync(UserRequest request, bool IsNewGuid = false)
        {
            if (await _context.Users.AnyAsync(u => u.Id == request.Id))
                throw new ForbiddenException("Пользователь существует");

            User user = request.ToEntity(IsNewGuid: IsNewGuid);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }

        public async Task<UserResponse> DeleteAsync(Guid id)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }
    }
}
