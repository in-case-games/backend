using Infrastructure.MassTransit.User;
using MassTransit;
using Payment.BLL.Interfaces;
using Payment.DAL.Entities;

namespace Payment.BLL.MassTransit.Consumers
{
    public class UserPromocodeConsumer : IConsumer<UserPromocodeTemplate>
    {
        private readonly IUserPromocodeService _promocodeService;

        public UserPromocodeConsumer(IUserPromocodeService promocodeService)
        {
            _promocodeService = promocodeService;
        }

        public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
        {
            var template = context.Message;

            if (template.Type?.Name == "balance")
            {
                UserPromocode? userPromocode = await _promocodeService
                    .GetAsync(template.Id, template.UserId);

                if (userPromocode is null)
                    await _promocodeService.CreateAsync(template);
                else
                    await _promocodeService.UpdateAsync(template);
            }
        }
    }
}
