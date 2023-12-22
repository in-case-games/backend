using Game.BLL.Exceptions;
using Game.DAL.Data;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class UserInventoryBackConsumer : IConsumer<UserInventoryBackTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserInventoryBackConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserInventoryBackTemplate> context)
        {
            var info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == context.Message.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.Balance += context.Message.FixedCost;

            await _context.SaveChangesAsync();
        }
    }
}
