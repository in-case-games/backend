using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services;

public class UserRoleService(ApplicationDbContext context) : IUserRoleService
{
    public async Task<UserRoleResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var role = await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Роль не найдена");

        return role.ToResponse();
    }

    public async Task<List<UserRoleResponse>> GetAsync(CancellationToken cancellation = default)
    {
        var roles = await context.Roles
            .AsNoTracking()
            .ToListAsync(cancellation);

        return roles.ToResponse();
    }
}