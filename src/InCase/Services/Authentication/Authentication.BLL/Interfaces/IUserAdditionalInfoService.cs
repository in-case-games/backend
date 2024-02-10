using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Interfaces;
public interface IUserAdditionalInfoService
{
    public Task UpdateAsync(UserAdditionalInfoTemplate template, CancellationToken cancellationToken = default);
}