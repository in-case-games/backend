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
        private readonly WithdrawItemService _withdrawService;
        private readonly EncryptorService _rsaService;
        private readonly GameMoneyService _gameMoneyService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            WithdrawItemService withdrawService,
            EncryptorService rsaService,
            GameMoneyService gameMoneyService)
        {
            _contextFactory = contextFactory;
            _withdrawService = withdrawService;
            _rsaService = rsaService;
            _gameMoneyService = gameMoneyService;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("withdrawn")]
        public async Task<IActionResult> WithdrawItem(DataWithdrawItem data)
        {
            //Transfer for bot await _gameMoneyService.TransferMoneyToTradeMarket(item.Cost / 7);
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? inventory = await context.UserInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.ItemId == data.ItemId && f.UserId == UserId);

            if (inventory == null)
                return ResponseUtil.NotFound("User inventory");

            GameItem item = inventory.Item!;

            item.Game = await context.Games
                .Include(i => i.Markets)
                .AsNoTracking()
                .FirstAsync(f => f.Id == item.GameId);

            ItemInfo itemInfo = await _withdrawService.GetItemInfo(item);

            decimal itemInfoPrice = itemInfo.Price / 7;

            if (itemInfoPrice > item.Cost * 1.1M)
                return ResponseUtil.Conflict("Item no stability price, exchange");

            decimal balance = await _withdrawService.GetBalance(itemInfo.Market);

            if (balance <= itemInfoPrice) 
                return ResponseUtil.Conflict("Wait payment");

            BuyItem buyItem = await _withdrawService.BuyItem(itemInfo, data.TradeUrl!);

            if (buyItem.Result != "OK")
                return ResponseUtil.Conflict("Unknown error");

            UserHistoryWithdrawn withdrawn = new()
            {
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = UserId
            };

            await context.UserHistoryWithdrawns.AddAsync(withdrawn);
            context.UserInventories.Remove(inventory);
            await context.SaveChangesAsync();

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
        public async Task<IActionResult> TopUpBalance(ResponsePaymentGM answer)
        {
            if (answer.StatusAnswer != "success")
                return ResponseUtil.Conflict(answer.ParametersAnswer ?? "Error");

            byte[] hashOfDataInSignIn = Encoding.ASCII.GetBytes(answer.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(answer.SignatureRSA!);

            if (!_rsaService.VerifySignatureRSA(hashOfDataInSignIn, signature))
                return Forbid("Poshel hacker lesom");

            ResponseInvoiceStatusGM? invoiceInfoStatus = await _gameMoneyService
                .GetInvoiceStatusInfo(answer.Invoice);

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
        [HttpGet("balance/{currency}")]
        public async Task<IActionResult> GetTerminalBalance(string currency)
        {
            ResponseBalanceGM? answerBalanceInfoGM = await _gameMoneyService.GetBalance(currency);

            return answerBalanceInfoGM is null ?
                ResponseUtil.NotFound(nameof(ResponseBalanceGM)) : 
                ResponseUtil.Ok(answerBalanceInfoGM);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("market/balance")]
        public async Task<IActionResult> GetMarketBalance(GameMarket market)
        {
            return ResponseUtil.Ok(await _withdrawService.GetBalance(market));
        }
    }
}
