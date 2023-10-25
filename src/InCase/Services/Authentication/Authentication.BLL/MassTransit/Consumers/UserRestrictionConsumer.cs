using Authentication.BLL.Interfaces;
using Authentication.DAL.Entities;
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
            var template = context.Message;

            UserRestriction? restriction = await _restrictionService.GetAsync(template.Id);

            if (restriction is null)
                await _restrictionService.CreateAsync(template);
            else if (template.IsDeleted)
                await _restrictionService.DeleteAsync(template.Id);
            else
                await _restrictionService.UpdateAsync(template);
        }
    }
}
