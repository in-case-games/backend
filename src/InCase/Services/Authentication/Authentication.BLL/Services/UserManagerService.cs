using Authentication.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authentication.BLL.Services
{
    public class UserManagerService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserManagerService> _logger;

        public UserManagerService(IServiceProvider serviceProvider, ILogger<UserManagerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = DoWork(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var userService = scope.ServiceProvider.GetService<IUserService>();
                    await userService!.DoWorkManagerAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
