using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.MassTransit.Consumers
{
    public class EmailConsumer : IConsumer
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public EmailConsumer(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailTemplate> context)
        {
            var data = context.Message;

            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstAsync(u => u.Email == data.Email);

            if(user.AdditionalInfo!.IsNotifyEmail || data.IsRequiredMessage)
                await _emailService.SendToEmailAsync(data);
        }
    }
}
