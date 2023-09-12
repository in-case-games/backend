using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;
using Identity.BLL.Helpers;
using Identity.BLL.MassTransit;
using Microsoft.AspNetCore.Http;
using Infrastructure.Services;

namespace Identity.BLL.Services
{
    public class UserAdditionalInfoService : IUserAdditionalInfoService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public UserAdditionalInfoService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
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
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateDeletionDateAsync(Guid userId, DateTime? deletionDate)
        {
            if (deletionDate is not null && deletionDate <= DateTime.UtcNow)
                throw new BadRequestException("Дата не корректна");

            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            info.DeletionDate = deletionDate;

            await _context.SaveChangesAsync();

            await _publisher.SendAsync(info.ToTemplate());

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateImageAsync(Guid userId, IFormFile image)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            bool isUploaded = FileService.Upload(image,
                path + $"userinfos\\{info.UserId}\\{info.Id}\\" + info.Id + ".jpg");

            if (!isUploaded)
                throw new ConflictException("Изображение не было загружено");

            await _context.SaveChangesAsync();

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateRoleAsync(Guid userId, Guid roleId)
        {
            UserRole role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == roleId) ??
                throw new NotFoundException("Роль не найдена");
            UserAdditionalInfo info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            info.RoleId = role.Id;

            await _context.SaveChangesAsync();

            info.Role = role;

            await _publisher.SendAsync(info.ToTemplate());

            return info.ToResponse();
        }
    }
}