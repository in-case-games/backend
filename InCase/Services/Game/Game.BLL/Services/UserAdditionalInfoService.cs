using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class UserAdditionalInfoService : IUserAdditionalInfoService
    {
        private readonly ApplicationDbContext _context;

        public UserAdditionalInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GuestModeResponse> GetGuestModeAsync(Guid userId)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ?? 
                throw new NotFoundException("Пользователь не найден");

            return info.ToGuestModeResponse();
        }

        public async Task<BalanceResponse> GetBalanceAsync(Guid userId)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToBalanceResponse();
        }

        public async Task<GuestModeResponse> ChangeGuestModeAsync(Guid userId)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            info.IsGuestMode = !info.IsGuestMode;

            await _context.SaveChangesAsync();

            return info.ToGuestModeResponse();
        }
    }
}
