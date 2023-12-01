using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Helpers;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.Services
{
    public class UserAdditionalInfoService : IUserAdditionalInfoService
    {
        private readonly ApplicationDbContext _context;

        public UserAdditionalInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == id, cancellationToken) ??
                throw new NotFoundException("Информация не найдена");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateNotifyEmailAsync(Guid userId, bool isNotifyEmail, CancellationToken cancellationToken = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellationToken) ??
                throw new NotFoundException("Информация не найдена");

            info.IsNotifyEmail = isNotifyEmail;

            await _context.SaveChangesAsync(cancellationToken);

            return info.ToResponse();
        }
    }
}
