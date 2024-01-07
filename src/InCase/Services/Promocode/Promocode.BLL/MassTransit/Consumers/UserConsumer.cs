﻿using Infrastructure.MassTransit.User;
using MassTransit;
using Promocode.BLL.Interfaces;

namespace Promocode.BLL.MassTransit.Consumers
{
    public class UserConsumer : IConsumer<UserTemplate>
    {
        private readonly IUserService _userService;

        public UserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserTemplate> context)
        {
            var user = await _userService.GetAsync(context.Message.Id);

            if (user is null) await _userService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _userService.DeleteAsync(user.Id);
        }
    }
}
