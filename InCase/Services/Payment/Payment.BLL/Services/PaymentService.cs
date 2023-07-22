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

        public async Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request)
        {
            if (request.StatusAnswer?.ToLower() != "paid")
                throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request))
                throw new ForbiddenException("Неверная подпись rsa");

            if (!await _context.Payments.AnyAsync(up => up.InvoiceId == request.InvoiceId!))
                throw new ConflictException("Платеж уже есть в системе, ждем пополнения");

            GameMoneyInvoiceInfoResponse? invoice = await _gmService
                .GetInvoiceInfoAsync(request.InvoiceId!) ?? 
                throw new Exceptions.RequestTimeoutException("Платеж не найден");

            UserPromocode? promocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == invoice.UserId);

            decimal pay = invoice.Amount;

            SiteStatisticsAdminTemplate templateStats = new() { TotalReplenished = pay };

            await _publisher.SendAsync(templateStats, "/statistics_admin");

            if (promocode is not null)
            {
                UserPromocodeTemplate templatePromo = promocode.ToTemplate();

                await _publisher.SendAsync(templatePromo, "/user-promocode_activated");

                _context.UserPromocodes.Remove(promocode);

                pay += pay * promocode.Discount;
            }

            //TODO Notify true game money

            UserPayment payment = new()
            {
                Amount = pay,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(invoice.Time),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                UserId = invoice.UserId
            };

            await _publisher.SendAsync(payment.ToTemplate(), "/user-payment");

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment.ToResponse();
        }

        public async Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency) => 
            await _gmService.GetBalanceAsync(currency);

        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) =>
            _gmService.GetHashOfDataForDeposit(userId);
    }
}
