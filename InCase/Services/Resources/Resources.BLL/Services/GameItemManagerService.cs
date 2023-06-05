using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resources.BLL.Interfaces;

namespace Resources.BLL.Services
{
    public class GameItemManagerService : IHostedService
    {
        private readonly IGameItemService _gameItemService;
        private readonly ILogger<GameItemManagerService> _logger;

        public GameItemManagerService(
            IServiceProvider serviceProvider, 
            ILogger<GameItemManagerService> logger)
        {
            _gameItemService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IGameItemService>();
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
                    _logger.LogInformation("Start work manager");
                    await _gameItemService.UpdateCostManagerAsync(10, stoppingToken);
                }
                catch (Exception)
                {
                    _logger.LogInformation("BB work manager");
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
