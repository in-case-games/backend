using Game.BLL.Exceptions;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class UserInventoryConsumer : IConsumer<UserInventoryTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserInventoryConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserInventoryTemplate> context)
        {
            var data = context.Message;

            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == data.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.Balance += data.FixedCost;

            await _context.SaveChangesAsync();
        }
    }
}
