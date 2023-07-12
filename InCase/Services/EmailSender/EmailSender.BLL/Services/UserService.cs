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

        public async Task<UserAdditionalInfoResponse> GetAsync(Guid id)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == id) ??
                throw new NotFoundException("Информация не найдена");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid id)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == id) ??
                throw new NotFoundException("Информация не найдена");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateNotifyEmailAsync(Guid userId, bool isNotifyEmail)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Информация не найдена");

            info.IsNotifyEmail = isNotifyEmail;

            await _context.SaveChangesAsync();

            return info.ToResponse();
        }
    }
}
