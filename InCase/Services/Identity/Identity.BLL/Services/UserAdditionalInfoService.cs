using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;
using Identity.BLL.Helpers;

namespace Identity.BLL.Services
{
    public class UserAdditionalInfoService : IUserAdditionalInfoService
    {
        private readonly ApplicationDbContext _context;

        public UserAdditionalInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserAdditionalInfoResponse> GetAsync(Guid id)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == id) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateDeletionDate(UserAdditionalInfoRequest request)
        {
            if (request.DeletionDate is not null && request.DeletionDate <= DateTime.UtcNow)
                throw new BadRequestException("Дата не корректна");

            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.Id == request.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.DeletionDate = request.DeletionDate;

            await _context.SaveChangesAsync();

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateImage(UserAdditionalInfoRequest request)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.Id == request.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.ImageUri = request.ImageUri;

            await _context.SaveChangesAsync();

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateRole(UserAdditionalInfoRequest request)
        {
            UserRole role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == request.RoleId) ??
                throw new NotFoundException("Роль не найдена");
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.Id == request.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.RoleId = role.Id;

            await _context.SaveChangesAsync();

            return info.ToResponse();
        }
    }
}