using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resources.BLL.Interfaces;

namespace Resources.BLL.Services
{
    public class GameItemManagerService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameItemManagerService> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public GameItemManagerService(
            IServiceProvider serviceProvider, 
            ILogger<GameItemManagerService> logger,
            IHostApplicationLifetime lifetime)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = DoWork(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task DoWork(CancellationToken cancellationToken)
        {
            if(!await WaitForAppStartup(cancellationToken)) return;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var gameItemService = scope.ServiceProvider.GetService<IGameItemService>();
                    await gameItemService!.UpdateCostManagerAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                    _logger.LogCritical(ex, ex.StackTrace);
                }

                await Task.Delay(1000, cancellationToken);
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
