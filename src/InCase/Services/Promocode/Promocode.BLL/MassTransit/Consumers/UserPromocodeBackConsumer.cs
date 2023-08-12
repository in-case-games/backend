﻿using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.MassTransit.Consumers
{
    public class UserPromocodeBackConsumer : IConsumer<UserPromocodeBackTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserPromocodeBackConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserPromocodeBackTemplate> context)
        {
            var template = context.Message;

            UserPromocode? promocode = await _context.UserPromocodes
                .FirstOrDefaultAsync(ur => ur.Id == template.Id);

            if(promocode is not null)
            {
                promocode.IsActivated = true;

                await _context.SaveChangesAsync();
            }
        }
    }
}