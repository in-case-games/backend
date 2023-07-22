using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.MassTransit.Consumers
{
    public class UserPromocodeConsumer : IConsumer<UserPromocodeTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserPromocodeConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
        {
            var template = context.Message;

            UserPromocode? promocode = await _context.UserPromocodes
                .FirstOrDefaultAsync(ur => ur.Id == template.Id);

            if(promocode is not null)
            {
                promocode.IsActivated = true;

                await _context.SaveChangesAsync();
            }
        }
    }
}
