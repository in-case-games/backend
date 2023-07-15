using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Helpers;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.Services
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
            if (await _context.Users.AnyAsync(u => u.Id == request.Id || u.Email == request.Email))
                throw new ForbiddenException("Пользователь существует");

            User user = request.ToEntity(IsNewGuid: IsNewGuid);

            UserAdditionalInfo info = new()
            {
                IsNotifyEmail = true,
                UserId = request.Id,
            };

            await _context.Users.AddAsync(user);
            await _context.AdditionalInfos.AddAsync(info);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateAsync(UserRequest request)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id) ??
                throw new NotFoundException("Пользователь не найден");

            user.Email = request.Email;

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
