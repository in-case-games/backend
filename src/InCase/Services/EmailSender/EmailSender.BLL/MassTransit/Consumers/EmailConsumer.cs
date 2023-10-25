﻿using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.Email;
using MassTransit;

namespace EmailSender.BLL.MassTransit.Consumers
{
    public class EmailConsumer : IConsumer<EmailTemplate>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public EmailConsumer(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailTemplate> context)
        {
            var template = context.Message;

            User? user = await _userService.GetAsync(template.Email);

            if(user is null || user.AdditionalInfo!.IsNotifyEmail || template.IsRequiredMessage)
                await _emailService.SendToEmailAsync(template);
        }
    }
}
