using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Data;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services;
public class UserAdditionalInfoService(ApplicationDbContext context) : IUserAdditionalInfoService
{
    public async Task UpdateAsync(UserAdditionalInfoTemplate template, CancellationToken cancellationToken = default)
    {
        var info = await context.UserAdditionalInfos
            .FirstOrDefaultAsync(uai => uai.UserId == template.UserId, cancellationToken) ?? 
            throw new NotFoundException("Пользователь не найден");

        var role = await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Name == template.RoleName, cancellationToken);

        info.DeletionDate = template.DeletionDate;

        if(role is not null) info.RoleId = role.Id;

        await context.SaveChangesAsync(cancellationToken);
    }
}