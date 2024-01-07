using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentication.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public UserService(ILogger<UserService> logger, ApplicationDbContext context, BasePublisher publisher)
        {
            _logger = logger;
            _context = context;
            _publisher = publisher;
        }

        public async Task DoWorkManagerAsync(CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .Where(uai => uai.AdditionalInfo!.DeletionDate <= DateTime.UtcNow)
                .Take(10)
                .ToListAsync(cancellationToken);

            foreach (var user in users)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                await _publisher.SendAsync(user.ToTemplate(true), cancellationToken);

                try
                {
                    FileService.RemoveFolder($"users/{user.Id}/");
                }
                catch (ConflictException ex)
                {
                    _logger.LogError($"UserId - {user.Id} Не удалось удалить папку");
                    _logger.LogError(ex, ex.Message);
                    _logger.LogError(ex, ex.StackTrace);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"UserId - {user.Id} не ожиданная ошибка на удаление папки");
                    _logger.LogCritical(ex, ex.Message);
                    _logger.LogCritical(ex, ex.StackTrace);
                }
            }
        }
    }
}
