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
        private readonly IHostApplicationLifetime _lifetime;

        public UserManagerService(
            IServiceProvider serviceProvider,
            IHostApplicationLifetime lifetime,
            ILogger<UserManagerService> logger)
        {
            _serviceProvider = serviceProvider;
            _lifetime = lifetime;
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
            if (!await WaitForAppStartup(stoppingToken)) return;

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
                    _logger.LogCritical(ex, ex.Message);
                    _logger.LogCritical(ex, ex.StackTrace);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<bool> WaitForAppStartup(CancellationToken stoppingToken)
        {
            var startedSource = new TaskCompletionSource();
            await using var reg1 = _lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

            var cancelledSource = new TaskCompletionSource();
            await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

            var completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

            return completedTask == startedSource.Task;
        }
    }
}
