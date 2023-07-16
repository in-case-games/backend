﻿using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.Email;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailSender.BLL.MassTransit.Consumers
{
    public class EmailConsumer : IConsumer<EmailTemplate>
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public EmailConsumer(
            ApplicationDbContext context, 
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailTemplate> context)
        {
            EmailTemplate template = context.Message;

            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == template.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if(user.AdditionalInfo!.IsNotifyEmail || template.IsRequiredMessage)
                await _emailService.SendToEmailAsync(template);
        }
    }
}
