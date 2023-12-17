using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;
using Identity.BLL.Helpers;
using Identity.BLL.MassTransit;
using ImageMagick;
using Infrastructure.MassTransit.User;

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

        public async Task<UserAdditionalInfoResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default)
        {
            var info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateDeletionDateAsync(Guid userId, DateTime? deletionDate, CancellationToken cancellation = default)
        {
            if (deletionDate is not null && deletionDate <= DateTime.UtcNow)
                throw new BadRequestException("Дата не корректна");

            var info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            info.DeletionDate = deletionDate;

            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(new UserAdditionalInfoTemplate()
            {
                Id = info.Id,
                DeletionDate = info.DeletionDate,
                RoleName = info.Role?.Name,
                UserId = info.UserId,
            }, cancellation);

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateImageAsync(UpdateImageRequest request, CancellationToken cancellation = default)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base64");

            var info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == request.UserId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            FileService.UploadImageBase64(request.Image, @$"users/{info.UserId}/", $"{info.UserId}");

            await _context.SaveChangesAsync(cancellation);

            return info.ToResponse();
        }

        public async Task<UserAdditionalInfoResponse> UpdateRoleAsync(Guid userId, Guid roleId, CancellationToken cancellation = default)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == roleId, cancellation) ??
                throw new NotFoundException("Роль не найдена");
            var info = await _context.AdditionalInfos
                .Include(uai => uai.Role)
                .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            info.RoleId = role.Id;

            await _context.SaveChangesAsync(cancellation);

            info.Role = role;

            await _publisher.SendAsync(new UserAdditionalInfoTemplate()
            {
                Id = info.Id,
                DeletionDate = info.DeletionDate,
                RoleName = info.Role?.Name,
                UserId = info.UserId,
            }, cancellation);

            return info.ToResponse();
        }
    }
}