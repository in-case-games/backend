﻿using Game.BLL.Exceptions;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class UserPaymentConsumer : IConsumer<UserPaymentTemplate>
    {
        private readonly ApplicationDbContext _context;

        public UserPaymentConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserPaymentTemplate> context)
        {
            var data = context.Message;

            UserAdditionalInfo info = await _context.AdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == data.UserId) ??
                throw new NotFoundException("Пользователь не найден");

            //TODO Check currency and rate
            info.Balance += data.Amount;

            await _context.SaveChangesAsync();
        }
    }
}