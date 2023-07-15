using EmailSender.BLL.Helpers;
using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailSender.BLL.MassTransit.Consumers
{
    public class UserConsumer : IConsumer<UserTemplate>
    {
        private readonly ILogger<UserConsumer> _logger;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UserConsumer(
            ILogger<UserConsumer> logger,
            IUserService userService, 
            ApplicationDbContext context)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserTemplate> context)
        {
            UserTemplate template = context.Message;

            _logger.LogInformation($"id: {template.Id} email: {template.Email} login: {template.Login}");

            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == template.Id);

            if (user is null)
                await _userService.CreateAsync(template.ToRequest());
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
            else if (user.Email != template.Email)
                await _userService.UpdateAsync(template.ToRequest());
        }
    }
}
