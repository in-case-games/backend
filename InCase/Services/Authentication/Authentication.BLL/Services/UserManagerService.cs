using Authentication.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authentication.BLL.Services
{
    public class UserManagerService : IHostedService
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserManagerService> _logger;

        public UserManagerService(
            IServiceProvider serviceProvider,
            ILogger<UserManagerService> logger)
        {
            _userService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IUserService>();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = DoWork(cancellationToken);

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Start work manager");
                    await _userService.DoWorkManagerAsync(stoppingToken);
                }
                catch (Exception)
                {
                    _logger.LogInformation("BB work manager");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
