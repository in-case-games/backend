using Microsoft.EntityFrameworkCore;
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
        private readonly EncryptorService _rsaService;

        public PaymentService(
            IGameMoneyService gameMoneyService,
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EncryptorService rsaService)
        {

            _gameMoneyService = gameMoneyService;
            _contextFactory = contextFactory;
            _rsaService = rsaService;

        }

        public async Task<UserPaymentsResponse> TopUpBalance(GameMoneyTopUpResponse request)
        {
            if (request.StatusAnswer == "cancel")
                throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request))
                throw new ForbiddenException("Неверная подпись rsa");

            GameMoneyInvoiceInfoResponse? invoice = await _gameMoneyService
                .GetInvoiceInfo(request.InvoiceId!);
            await using ApplicationDbContext context = await _contextFactory
                .CreateDbContextAsync();

            string nameStatus = invoice.Status!.Replace("_", "-").ToLower();

            PaymentInvoiceStatus status = await context.PaymentsStatuses
                .AsNoTracking()
                .FirstAsync(ips => ips.Name == nameStatus);

            UserPayments payment = new()
            {
                Amount = invoice.Amount,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(request.SendTimeAnswer),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                StatusId = status.Id,
                UserId = invoice.UserId
            };

            await context.UserPayments.AddAsync(payment);
            await context.SaveChangesAsync();

            return payment.ToResponse();
        }

        public async Task<decimal> GetPaymentBalance(string currency)
        {
            return await _gameMoneyService.GetBalance(currency);
        }

        public string GetHashOfDataForDeposit(Guid userId)
        {
            return _gameMoneyService.GetHashOfDataForDeposit(userId);
        }
    }
}
