using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.MassTransit;
using Payment.BLL.Models;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGameMoneyService _gmService;
        private readonly ApplicationDbContext _context;
        private readonly IEncryptorService _rsaService;
        private readonly BasePublisher _publisher;

        public PaymentService(
            IGameMoneyService gmService,
            ApplicationDbContext context,
            IEncryptorService rsaService,
            BasePublisher publisher)
        {

            _gmService = gmService;
            _context = context;
            _rsaService = rsaService;
            _publisher = publisher;
        }

        public async Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request, CancellationToken cancellation = default)
        {
            if (request.StatusAnswer?.ToLower() != "paid") throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request)) throw new ForbiddenException("Неверная подпись rsa");
            if (!await _context.Payments.AnyAsync(up => up.InvoiceId == request.InvoiceId!, cancellation))
                throw new ConflictException("Платеж уже есть в системе, ждем пополнения");

            var invoice = await _gmService.GetInvoiceInfoAsync(request.InvoiceId!, cancellation) ?? 
                throw new RequestTimeoutException("Платеж не найден");

            var promocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == invoice.UserId, cancellation);

            var pay = invoice.Amount;

            await _publisher.SendAsync(new SiteStatisticsAdminTemplate { 
                TotalReplenishedFunds = pay 
            }, cancellation);

            if (promocode is not null)
            {
                await _publisher.SendAsync(new UserPromocodeBackTemplate
                {
                    Id = promocode.Id
                }, cancellation);

                _context.UserPromocodes.Remove(promocode);

                pay += pay * promocode.Discount;
            }

            var payment = new UserPayment()
            {
                Amount = pay,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(invoice.Time),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                UserId = invoice.UserId
            };

            // CHECK: Notify true game money
            await _gmService.SendSuccess(cancellation);
            await _context.Payments.AddAsync(payment, cancellation);
            await _publisher.SendAsync(new UserPaymentTemplate
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Date = payment.Date,
                Rate = payment.Rate,
                UserId = payment.UserId
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return payment.ToResponse();
        }

        public async Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency, 
            CancellationToken cancellation = default) => await _gmService.GetBalanceAsync(currency, cancellation);

        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) =>
            _gmService.GetHashOfDataForDeposit(userId);
    }
}
