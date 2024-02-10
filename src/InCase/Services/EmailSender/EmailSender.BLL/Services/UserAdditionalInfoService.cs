using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using EmailSender.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.Services;
public class UserAdditionalInfoService(ApplicationDbContext context) : IUserAdditionalInfoService
{
    public async Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var info = await context.UserAdditionalInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(uai => uai.UserId == id, cancellationToken) ??
            throw new NotFoundException("Информация не найдена");

        return new UserAdditionalInfoResponse()
        {
            Id = info.Id,
            IsNotifyEmail = info.IsNotifyEmail,
            UserId = info.UserId,
        };
    }

    public async Task<UserAdditionalInfoResponse> UpdateNotifyEmailAsync(Guid userId, bool isNotifyEmail, CancellationToken cancellationToken = default)
    {
        var info = await context.UserAdditionalInfos
            .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellationToken) ??
            throw new NotFoundException("Информация не найдена");

        info.IsNotifyEmail = isNotifyEmail;

        await context.SaveChangesAsync(cancellationToken);

        return new UserAdditionalInfoResponse()
        {
            Id = info.Id,
            IsNotifyEmail = info.IsNotifyEmail,
            UserId = info.UserId,
        };
    }
}