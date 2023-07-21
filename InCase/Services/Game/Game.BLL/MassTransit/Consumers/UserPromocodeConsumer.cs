using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
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

            if (template.Type?.Name == "box")
            {
                UserPromocode? userPromocode = await _context.UserPromocodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ur => ur.Id == template.Id && ur.UserId == template.UserId);

                if (userPromocode is null)
                    await _promocodeService.CreateAsync(template.ToRequest());
                else
                    await _promocodeService.UpdateAsync(template.ToRequest());
            }
        }
    }
}
