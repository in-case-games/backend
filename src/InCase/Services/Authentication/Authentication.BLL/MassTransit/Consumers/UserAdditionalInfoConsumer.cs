using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Authentication.BLL.MassTransit.Consumers;
public class UserAdditionalInfoConsumer(IUserAdditionalInfoService infoService) : IConsumer<UserAdditionalInfoTemplate>
{
    public async Task Consume(ConsumeContext<UserAdditionalInfoTemplate> context) =>
        await infoService.UpdateAsync(context.Message);
}