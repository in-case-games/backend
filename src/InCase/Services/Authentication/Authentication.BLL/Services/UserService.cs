using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentication.BLL.Services;
public class UserService(ILogger<UserService> logger, ApplicationDbContext context, BasePublisher publisher) : IUserService
{
    public async Task DoWorkManagerAsync(CancellationToken cancellationToken)
    {
        var users = await context.Users
            .Include(u => u.AdditionalInfo)
            .AsNoTracking()
            .Where(uai => uai.AdditionalInfo!.DeletionDate <= DateTime.UtcNow)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
            await publisher.SendAsync(user.ToTemplate(true), cancellationToken);

            try
            {
                FileService.RemoveFolder($"users/{user.Id}/");
            }
            catch (ConflictException ex)
            {
                logger.LogError($"UserId - {user.Id} Не удалось удалить папку");
                logger.LogError(ex, ex.Message);
                logger.LogError(ex, ex.StackTrace);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"UserId - {user.Id} не ожиданная ошибка на удаление папки");
                logger.LogCritical(ex, ex.Message);
                logger.LogCritical(ex, ex.StackTrace);
            }
        }
    }
}