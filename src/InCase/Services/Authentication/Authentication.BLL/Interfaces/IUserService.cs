namespace Authentication.BLL.Interfaces;
public interface IUserService
{
    public Task DoWorkManagerAsync(CancellationToken stoppingToken);
}