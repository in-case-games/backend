using Game.BLL.Exceptions;
using Game.DAL.Data;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class UserPaymentConsumer : IConsumer<UserPaymentTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserPaymentConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserPaymentTemplate> context)
        {
            var info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == context.Message.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.Balance += context.Message.Amount;

            await _context.SaveChangesAsync();
        }
    }
}
