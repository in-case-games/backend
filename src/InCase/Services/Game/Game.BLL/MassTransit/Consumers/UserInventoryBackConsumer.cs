using Game.BLL.Exceptions;
using Game.DAL.Data;
using Game.DAL.Entities;
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
            var data = context.Message;

            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == data.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            info.Balance += data.FixedCost;

            await _context.SaveChangesAsync();
        }
    }
}
