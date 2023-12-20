using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Authentication.BLL.MassTransit.Consumers
{
    public class UserRestrictionConsumer : IConsumer<UserRestrictionTemplate>
    {
        private readonly IUserRestrictionService _restrictionService;

        public UserRestrictionConsumer(IUserRestrictionService restrictionService)
        {
            _restrictionService = restrictionService;
        }

        public async Task Consume(ConsumeContext<UserRestrictionTemplate> context)
        {
            var restriction = await _restrictionService.GetAsync(context.Message.Id);

            if (restriction is null) await _restrictionService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _restrictionService.DeleteAsync(context.Message.Id);
            else await _restrictionService.UpdateAsync(context.Message);
        }
    }
}
