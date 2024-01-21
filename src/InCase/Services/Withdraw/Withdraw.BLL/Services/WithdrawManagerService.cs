using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.Services;

public class WithdrawManagerService(
    IServiceProvider serviceProvider, 
    ILogger<WithdrawManagerService> logger, 
    IHostApplicationLifetime lifetime) : IHostedService
{
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
                await using var scope = serviceProvider.CreateAsyncScope();

                var withdraw = scope.ServiceProvider.GetService<IWithdrawService>();

                await withdraw!.WithdrawStatusManagerAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                logger.LogCritical(ex, ex.StackTrace);
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    private async Task<bool> WaitForAppStartup(CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        await using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

        var cancelledSource = new TaskCompletionSource();
        await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        var completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

        return completedTask == startedSource.Task;
    }
}