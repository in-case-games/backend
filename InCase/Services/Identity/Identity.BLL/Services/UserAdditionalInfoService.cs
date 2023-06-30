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

        public async Task<UserAdditionalInfoResponse> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == id) ??
                throw new NotFoundException("Дополнительная информация не найдена");

            return info.ToResponse();
        }

        public async Task UpdateAsync(UserAdditionalInfoRequest request, CancellationToken cancellationToken = default)
        {
            if (!await _context.Roles.AnyAsync(ur => ur.Id == request.RoleId, cancellationToken))
                throw new NotFoundException("Роль не найдена");
            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            UserAdditionalInfo? entityOld = await _context.Set<UserAdditionalInfo>()
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken) ??
                throw new NotFoundException($"Запись таблицы {nameof(UserAdditionalInfo)} по {request.Id} не найдена");

            try
            {
                _context.Entry(entityOld).CurrentValues.SetValues(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new UnknownErrorException(ex);
            }
        }
    }
}