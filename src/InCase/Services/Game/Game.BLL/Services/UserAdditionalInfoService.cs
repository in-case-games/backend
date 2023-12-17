using Game.BLL.Exceptions;
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

        public async Task<GuestModeResponse> GetGuestModeAsync(Guid userId, CancellationToken cancellation = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ?? 
                throw new NotFoundException("Пользователь не найден");

            return new GuestModeResponse
            {
                IsGuestMode = info.IsGuestMode,
            };
        }

        public async Task<BalanceResponse> GetBalanceAsync(Guid userId, CancellationToken cancellation = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            return new BalanceResponse
            {
                Balance = info.Balance,
            };
        }

        public async Task<GuestModeResponse> ChangeGuestModeAsync(Guid userId, CancellationToken cancellation = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            info.IsGuestMode = !info.IsGuestMode;

            await _context.SaveChangesAsync(cancellation);

            return new GuestModeResponse
            {
                IsGuestMode = info.IsGuestMode,
            };
        }

        public async Task<BalanceResponse> ChangeBalanceByOwnerAsync(Guid userId, decimal balance, CancellationToken cancellation = default)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            info.Balance = balance;

            await _context.SaveChangesAsync(cancellation);

            return new BalanceResponse { 
                Balance = info.Balance,
            };
        }
    }
}
