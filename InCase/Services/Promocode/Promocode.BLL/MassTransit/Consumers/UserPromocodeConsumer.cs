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
            var data = context.Message;

            UserPromocode userPromocode = await _context.UserHistoriesPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == data.Id) ??
                throw new NotFoundException("Промокод пользователя не найден");

            userPromocode.IsActivated = true;

            await _context.SaveChangesAsync();
        }
    }
}
