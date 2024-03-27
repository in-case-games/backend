using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Authentication.BLL.MassTransit.Consumers;
public class UserRestrictionConsumer(IUserRestrictionService restrictionService) : IConsumer<UserRestrictionTemplate>
{
    public async Task Consume(ConsumeContext<UserRestrictionTemplate> context)
    {
        var restriction = await restrictionService.GetAsync(context.Message.Id);

        if (restriction is not null && context.Message.IsDeleted) await restrictionService.DeleteAsync(restriction.Id);
        else if(!context.Message.IsDeleted) {
            if (restriction is null) await restrictionService.CreateAsync(context.Message);
            else await restrictionService.UpdateAsync(context.Message);
        }
    }
}