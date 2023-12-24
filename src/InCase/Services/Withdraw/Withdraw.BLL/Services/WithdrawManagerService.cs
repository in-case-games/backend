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
        private readonly IHostApplicationLifetime _lifetime;

        public WithdrawManagerService(
            IServiceProvider serviceProvider, 
            ILogger<WithdrawManagerService> logger,
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
            if (!await WaitForAppStartup(cancellationToken)) return;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();

                    var withdraw = scope.ServiceProvider.GetService<IWithdrawService>();

                    await withdraw!.WithdrawStatusManagerAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                    _logger.LogCritical(ex, ex.StackTrace);
                }

                await Task.Delay(500, cancellationToken);
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
