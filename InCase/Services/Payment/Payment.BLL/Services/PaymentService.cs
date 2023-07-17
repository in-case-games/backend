using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly ApplicationDbContext _context;
        private readonly IEncryptorService _rsaService;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public PaymentService(
            IGameMoneyService gameMoneyService,
            ApplicationDbContext context,
            IEncryptorService rsaService,
            IConfiguration configuration,
            IBus bus)
        {

            _gameMoneyService = gameMoneyService;
            _context = context;
            _rsaService = rsaService;
            _configuration = configuration;
            _bus = bus;
        }

        public async Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request)
        {
            if (request.StatusAnswer?.ToLower() != "paid")
                throw new BadRequestException("Платеж отклонен");
            if (!_rsaService.VerifySignatureRSA(request))
                throw new ForbiddenException("Неверная подпись rsa");

            if (!await _context.UserPayments.AnyAsync(up => up.InvoiceId == request.InvoiceId!))
                throw new ConflictException("Платеж уже есть в системе, ждем пополнения");

            GameMoneyInvoiceInfoResponse? invoice = await _gameMoneyService
                .GetInvoiceInfoAsync(request.InvoiceId!) ?? 
                throw new Exceptions.RequestTimeoutException("Платеж не найден");

            UserPromocode? promocode = await _context.UsersPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == invoice.UserId);

            decimal pay = invoice.Amount;

            if (promocode is not null)
            {
                UserPromocodeTemplate templatePromo = promocode.ToTemplate();

                Uri uriPromo = new(_configuration["MassTransit:Uri"] + "/user-promocode_activated");
                var endPointPromo = await _bus.GetSendEndpoint(uriPromo);
                await endPointPromo.Send(templatePromo);

                _context.UsersPromocodes.Remove(promocode);

                pay += pay * promocode.Discount;
            }

            //TODO Notify true game money

            UserPayment payment = new()
            {
                Amount = invoice.Amount,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(invoice.Time),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                UserId = invoice.UserId
            };

            Uri uriPayment = new(_configuration["MassTransit:Uri"] + "/user-payment");
            var endPointPayment = await _bus.GetSendEndpoint(uriPayment);
            await endPointPayment.Send(payment.ToTemplate());

            await _context.UserPayments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment.ToResponse();
        }

        public async Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency) => 
            await _gameMoneyService.GetBalanceAsync(currency);

        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) =>
            _gameMoneyService.GetHashOfDataForDeposit(userId);
    }
}
