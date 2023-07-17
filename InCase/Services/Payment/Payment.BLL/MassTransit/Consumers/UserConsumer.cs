using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.MassTransit.Consumers
{
    public class UserConsumer : IConsumer<UserTemplate>
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UserConsumer(
            IUserService userService,
            ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserTemplate> context)
        {
            UserTemplate template = context.Message;

            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == template.Id);

            if (user is null)
                await _userService.CreateAsync(template.ToRequest());
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
        }
    }
}
