using Authentication.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication.BLL.Services
{
    public class UserManagerService : IHostedService
    {
        private readonly IUserService _userService;

        public UserManagerService(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IUserService>();
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
                    await _userService.DoWorkManagerAsync(stoppingToken);
                }
                catch (Exception)
                {
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
