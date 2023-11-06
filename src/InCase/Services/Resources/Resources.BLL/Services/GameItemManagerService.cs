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

        public GameItemManagerService(IServiceProvider serviceProvider, ILogger<GameItemManagerService> logger)
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
                    var gameItemService = scope.ServiceProvider.GetService<IGameItemService>();
                    await gameItemService!.UpdateCostManagerAsync(10, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
