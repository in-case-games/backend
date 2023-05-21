using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;
using Identity.BLL.Helpers;

namespace Identity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _context;

        public UserService(IdentityDbContext context)
        {
            _context = context;
        }
        public async Task<UserResponse> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            User user = await _context.Users
                .AsNoTracking()
                .Include(ui => ui.AdditionalInfo)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            return user.ToResponse();
        }
        public async Task<List<UserResponse>> GetAsync(int range = 100, CancellationToken cancellationToken = default)
        {
            if (range < 0 && range >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<User> users = await _context.Users
                .AsNoTracking()
                .Take(range)
                .ToListAsync(cancellationToken);

            return users.ToResponse();
        }
        public async Task<List<UserRoleResponse>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            List<UserRoleResponse> roles = await _context.UserRoles
                .AsNoTracking()
                .Select(r => new UserRoleResponse() { Id = r.Id, Name = r.Name})
                .ToListAsync(cancellationToken);

            return roles;
        }
        public async Task UpdateAsync(UserAdditionalInfoRequest info, CancellationToken cancellationToken = default)
        {
            if (!await _context.UserRoles.AnyAsync(ur => ur.Id == info.RoleId, cancellationToken))
                throw new NotFoundException("Роль не найдена");
            if (!await _context.Users.AnyAsync(u => u.Id == info.UserId, cancellationToken))
                throw new NotFoundException("Пользователь не найден");

            UserAdditionalInfo? entityOld = await _context.Set<UserAdditionalInfo>()
                .FirstOrDefaultAsync(f => f.Id == info.Id, cancellationToken) ??
                throw new NotFoundException($"Запись таблицы {nameof(UserAdditionalInfo)} по {info.Id} не найдена");

            try
            {
                _context.Entry(entityOld).CurrentValues.SetValues(info);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new UnknownErrorException(ex);
            }
        }
    }
}