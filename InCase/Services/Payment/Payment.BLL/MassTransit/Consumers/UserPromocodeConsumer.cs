using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.MassTransit.Consumers
{
    public class UserPromocodeConsumer : IConsumer<UserPromocodeTemplate>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserPromocodeService _promocodeService;

        public UserPromocodeConsumer(
            ApplicationDbContext context,
            IUserPromocodeService promocodeService)
        {
            _context = context;
            _promocodeService = promocodeService;
        }

        public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
        {
            UserPromocodeTemplate template = context.Message;

            if (template.Type?.Name == "balance")
            {
                UserPromocode? userPromocode = await _context.UserPromocodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId);

                if (userPromocode is null)
                    await _promocodeService.CreateAsync(template);
                else
                    await _promocodeService.UpdateAsync(template);
            }
        }
    }
}
