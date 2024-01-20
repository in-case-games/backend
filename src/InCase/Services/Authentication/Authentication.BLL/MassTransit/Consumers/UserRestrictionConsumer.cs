using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Authentication.BLL.MassTransit.Consumers
{
    public class UserRestrictionConsumer(IUserRestrictionService restrictionService) : IConsumer<UserRestrictionTemplate>
    {
        public async Task Consume(ConsumeContext<UserRestrictionTemplate> context)
        {
            var restriction = await restrictionService.GetAsync(context.Message.Id);

            if (restriction is null) await restrictionService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await restrictionService.DeleteAsync(context.Message.Id);
            else await restrictionService.UpdateAsync(context.Message);
        }
    }
}
