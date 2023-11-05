using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.Services
{
    public class WithdrawManagerService : IHostedService
    {
        private readonly IWithdrawService _withdrawService;
        private readonly ILogger<WithdrawManagerService> _logger;

        public WithdrawManagerService(IServiceProvider serviceProvider, ILogger<WithdrawManagerService> logger)
        {
            _withdrawService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IWithdrawService>();
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
                    await _withdrawService.WithdrawStatusManagerAsync(10, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
