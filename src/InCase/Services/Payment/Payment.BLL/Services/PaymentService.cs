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
            if (request.StatusAnswer?.ToLower() != "paid")
                throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request))
                throw new ForbiddenException("Неверная подпись rsa");

            if (!await _context.Payments.AnyAsync(up => up.InvoiceId == request.InvoiceId!, cancellation))
                throw new ConflictException("Платеж уже есть в системе, ждем пополнения");

            GameMoneyInvoiceInfoResponse? invoice = await _gmService
                .GetInvoiceInfoAsync(request.InvoiceId!, cancellation) ?? 
                throw new Exceptions.RequestTimeoutException("Платеж не найден");

            UserPromocode? promocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == invoice.UserId, cancellation);

            decimal pay = invoice.Amount;

            SiteStatisticsAdminTemplate templateStats = new() { TotalReplenishedFunds = pay };

            await _publisher.SendAsync(templateStats, cancellation);

            if (promocode is not null)
            {
                UserPromocodeBackTemplate templatePromo = promocode.ToTemplate();

                await _publisher.SendAsync(templatePromo, cancellation);

                _context.UserPromocodes.Remove(promocode);

                pay += pay * promocode.Discount;
            }

            // CHECK: Notify true game money
            await _gmService.SendSuccess(cancellation);

            UserPayment payment = new()
            {
                Amount = pay,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(invoice.Time),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                UserId = invoice.UserId
            };

            await _publisher.SendAsync(payment.ToTemplate(), cancellation);

            await _context.Payments.AddAsync(payment, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return payment.ToResponse();
        }

        public async Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency, CancellationToken cancellation = default) => 
            await _gmService.GetBalanceAsync(currency, cancellation);

        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) =>
            _gmService.GetHashOfDataForDeposit(userId);
    }
}
