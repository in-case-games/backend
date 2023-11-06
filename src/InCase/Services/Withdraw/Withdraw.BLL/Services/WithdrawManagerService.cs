using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.Services
{
    public class WithdrawManagerService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WithdrawManagerService> _logger;

        public WithdrawManagerService(IServiceProvider serviceProvider, ILogger<WithdrawManagerService> logger)
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

                    var withdraw = scope.ServiceProvider.GetService<IWithdrawService>();

                    await withdraw!.WithdrawStatusManagerAsync(10, stoppingToken);
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
