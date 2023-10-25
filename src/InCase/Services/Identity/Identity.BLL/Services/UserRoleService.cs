using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly ApplicationDbContext _context;

        public UserRoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRoleResponse> GetAsync(Guid id)
        {
            UserRole role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Роль не найдена");

            return role.ToResponse();
        }

        public async Task<List<UserRoleResponse>> GetAsync()
        {
            List<UserRole> roles = await _context.Roles
                .AsNoTracking()
                .ToListAsync();

            return roles.ToResponse();
        }
    }
}
