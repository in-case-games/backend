using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class UserAdditionalInfoService : IUserAdditionalInfoService
    {
        private readonly ApplicationDbContext _context;

        public UserAdditionalInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(UserAdditionalInfoTemplate template)
        {
            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == template.UserId) ?? 
                throw new NotFoundException("Пользователь не найден");

            UserRole? role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Name == template.RoleName);

            info.DeletionDate = template.DeletionDate;

            if(role is not null)
                info.RoleId = role.Id;

            await _context.SaveChangesAsync();
        }
    }
}
