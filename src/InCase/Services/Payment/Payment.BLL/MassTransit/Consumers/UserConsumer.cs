using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;

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
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == context.Message.Id);

            if (user is null) await _userService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _userService.DeleteAsync(user.Id);
        }
    }
}
