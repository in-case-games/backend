using InCase.Domain.Common;
using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace InCase.Payment.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly TradeMarketService _marketTMService;
        private readonly EncryptorService _rsaService;
        private readonly GameMoneyService _gameMoneyService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            TradeMarketService marketTMService,
            EncryptorService rsaService,
            GameMoneyService gameMoneyService)
        {
            _contextFactory = contextFactory;
            _marketTMService = marketTMService;
            _rsaService = rsaService;
            _gameMoneyService = gameMoneyService;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("withdrawn")]
        public async Task<IActionResult> WithdrawItem(DataWithdrawItem withdrawItem)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? inventory = await context.UserInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.ItemId == withdrawItem.ItemId && f.UserId == UserId);

            if (inventory == null)
                return ResponseUtil.NotFound("User inventory");

            GameItem item = inventory.Item!;

            item.Game = await context.Games
                .FirstAsync(f => f.Id == item.GameId);

            if (string.IsNullOrEmpty(item.IdForPlatform))
            {
                //TODO Notify admin by telegram auto withdrawn no work
                return ResponseUtil.Ok("Wait for the admin to accept");
            }

            //Check info item in tm
            ItemInfoTM? itemInfoTM = await _marketTMService.GetMarketItemInfo(item);

            if (itemInfoTM == null || itemInfoTM.Offers!.Count <= 0)
                return ResponseUtil.NotFound("Game item for platform");

            decimal minItemPriceTM = decimal.Parse(itemInfoTM.MinPrice!) / 100;

            if (minItemPriceTM > item.Cost / 7 * 1.1M)
                return ResponseUtil.Conflict("Item no stability price, exchange");

            //Check balance tm
            decimal balanceTM = await _marketTMService.GetTradeMarketInfo();

            if (balanceTM <= item.Cost / 7)
                return ResponseUtil.Conflict("Wait payment");

            await _gameMoneyService.TransferMoneyToTradeMarket(item.Cost / 7);

            //Buy item tm
            ResponseBuyItemTM? itemBuyTM = await _marketTMService.BuyMarketItem(
                item,
                withdrawItem.TradeUrl!,
                withdrawItem.TradeUrl!);

            if (itemBuyTM is null)
                return ResponseUtil.NotFound("Trade url");

            context.UserInventories.Remove(inventory);

            return ResponseUtil.Ok("Item was withdrawed");
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("deposit/signature")]
        public async Task<IActionResult> GetSignatureForDeposit()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string hash = _gameMoneyService.CreateHashOfDataForDeposit(UserId);
            string hmac = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return ResponseUtil.Ok(hmac);
        }

        [AllowAnonymous]
        [HttpPost("deposit")]
        public async Task<IActionResult> TopUpBalance(ResponsePaymentGM paymentAnswer)
        {
            if (paymentAnswer.StatusAnswer != "success")
                return ResponseUtil.Conflict(paymentAnswer.ParametersAnswer ?? "Error");

            byte[] hashOfDataInSignIn = Encoding.ASCII.GetBytes(paymentAnswer.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(paymentAnswer.SignatureRSA!);

            if (!_rsaService.VerifySignatureRSA(hashOfDataInSignIn, signature))
                return Forbid("Poshel hacker lesom");

            ResponseInvoiceStatusGM? invoiceInfoStatus = await _gameMoneyService
                .GetInvoiceStatusInfo(paymentAnswer.Invoice);

            if (invoiceInfoStatus is null || invoiceInfoStatus.Status != "Paid")
                return ResponseUtil.Ok("Payed. Wait some time for replenishment");

            byte[] signatureInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.SignatureRSA!);
            byte[] hashOfDataInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.ToString()!);

            if (!_rsaService.VerifySignatureRSA(hashOfDataInvoice, signatureInvoice))
                return Forbid("Poshel hacker lesom");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo info = (await context.UserAdditionalInfos
                .FirstOrDefaultAsync(x => x.UserId == invoiceInfoStatus.UserId))!;

            info.Balance += invoiceInfoStatus.Amount * 7; //TODO Check current answer

            await context.SaveChangesAsync();

            return ResponseUtil.Ok("Balance top up");
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("gamemoney/balance/{currency}")]
        public async Task<IActionResult> GetGameMoneyBalance(string currency)
        {
            ResponseBalanceGM? answerBalanceInfoGM = await _gameMoneyService.GetBalanceInfo(currency);

            return answerBalanceInfoGM is null ?
                ResponseUtil.NotFound(nameof(ResponseBalanceGM)) : 
                ResponseUtil.Ok(answerBalanceInfoGM);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("market/balance")]
        public async Task<IActionResult> GetTradeMarketBalance()
        {
            return ResponseUtil.Ok(await _marketTMService.GetTradeMarketInfo());
        }
    }
}
