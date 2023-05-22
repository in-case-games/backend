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

        public WithdrawManagerService(
            IServiceProvider serviceProvider,
            ILogger<WithdrawManagerService> logger)
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
                    _logger.LogInformation("Start work manager");
                    await _withdrawService.WithdrawStatusManagerAsync(10, stoppingToken);
                }
                catch (Exception)
                {
                    _logger.LogInformation("BB work manager");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
