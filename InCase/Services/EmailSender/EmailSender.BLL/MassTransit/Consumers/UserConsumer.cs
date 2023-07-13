using EmailSender.BLL.Models;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.MassTransit.Consumers
{
    public class UserConsumer : IConsumer<UserConsumerTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserConsumerTemplate> context)
        {
            await _context.Users.AnyAsync(u => u.Id == Guid.NewGuid()); //
        }
    }
}
