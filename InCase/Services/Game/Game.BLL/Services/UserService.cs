using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
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
                throw new NotFoundException("Пользователь существует");

            User user = request.ToEntity(IsNewGuid: IsNewGuid);

            UserAdditionalInfo info = new()
            {
                UserId = request.Id,
                Balance = 0,
                IsGuestMode = false,
            };

            await _context.Users.AddAsync(user);
            await _context.AdditionalInfos.AddAsync(info);
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
