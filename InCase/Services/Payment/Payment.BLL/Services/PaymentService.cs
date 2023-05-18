﻿using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGameMoneyService _gameMoneyService;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IEncryptorService _rsaService;

        public PaymentService(
            IGameMoneyService gameMoneyService,
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IEncryptorService rsaService)
        {

            _gameMoneyService = gameMoneyService;
            _contextFactory = contextFactory;
            _rsaService = rsaService;

        }

        public async Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request)
        {
            if (request.StatusAnswer == "cancel")
                throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request))
                throw new ForbiddenException("Неверная подпись rsa");

            await using ApplicationDbContext context = await _contextFactory
                .CreateDbContextAsync();

            if (!await context.UserPayments.AnyAsync(up => up.InvoiceId == request.InvoiceId!))
                throw new ConflictException("Платеж уже есть в системе, ждем пополнения");

            GameMoneyInvoiceInfoResponse? invoice = await _gameMoneyService
                .GetInvoiceInfoAsync(request.InvoiceId!);

            string nameStatus = invoice.Status!.Replace("_", "-").ToLower();

            UserPayment payment = new()
            {
                Amount = invoice.Amount,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(request.SendTimeAnswer),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                UserId = invoice.UserId
            };

            await context.UserPayments.AddAsync(payment);
            await context.SaveChangesAsync();

            return payment.ToResponse();
        }

        public async Task<decimal> GetPaymentBalanceAsync(string currency)
        {
            return await _gameMoneyService.GetBalanceAsync(currency);
        }

        public string GetHashOfDataForDeposit(Guid userId)
        {
            return _gameMoneyService.GetHashOfDataForDeposit(userId);
        }

        //TODO rename in top up balance
        public Task DoWorkManagerAsync(CancellationToken cancellationToken)
        {
            //TODO Logic activate promo and notify rabbit mq
            //TODO Logic payment is success status and notyfy rabbit mq

            throw new NotImplementedException();
        }
    }
}
