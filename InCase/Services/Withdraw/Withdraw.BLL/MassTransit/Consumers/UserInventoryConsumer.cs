using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.MassTransit.Consumers
{
    public class UserInventoryConsumer : IConsumer<UserInventoryTemplate>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserInventoryService _inventoryService;

        public UserInventoryConsumer(
            ApplicationDbContext context, 
            IUserInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public async Task Consume(ConsumeContext<UserInventoryTemplate> context)
        {
            var template = context.Message;

            UserInventory? inventory = await _context.UserInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == template.Id);

            if(inventory is null)
                await _inventoryService.CreateAsync(template);
        }
    }
}
