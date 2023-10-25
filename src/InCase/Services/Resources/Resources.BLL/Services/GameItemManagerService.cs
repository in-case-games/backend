using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resources.BLL.Interfaces;

namespace Resources.BLL.Services
{
    public class GameItemManagerService : IHostedService
    {
        private readonly IGameItemService _gameItemService;

        public GameItemManagerService(IServiceProvider serviceProvider)
        {
            _gameItemService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IGameItemService>();
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
                    await _gameItemService.UpdateCostManagerAsync(10, stoppingToken);
                }
                catch (Exception)
                {
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
