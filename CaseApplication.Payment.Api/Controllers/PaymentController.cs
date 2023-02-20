using CaseApplication.Domain.Entities.External;
using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Payment.Api.Controllers
{
    [Route("payment/api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MarketTMService _marketTMService;
        private readonly RSAService _rsaService;
        private readonly GameMoneyService _gameMoneyService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            MarketTMService marketTMService,
            RSAService rsaService,
            GameMoneyService gameMoneyService)
        {
            _contextFactory = contextFactory;
            _marketTMService = marketTMService;
            _rsaService = rsaService;
            _gameMoneyService = gameMoneyService;
        }

        [Authorize]
        [HttpGet("withdrawn")]
        public async Task<IActionResult> WithdrawItem(WithdrawItem withdrawItem)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? userInventory = await context.UserInventory
                .Include(c => c.GameItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameItemId == withdrawItem.GameItemId && x.UserId == UserId);

            if(userInventory == null) return NotFound("Item is not found in user invetory");
            GameItem gameItem = userInventory.GameItem!;

            if (gameItem.GameItemIdForPlatform is null)
            {
                //TODO Notify admin by telegram auto withdrawn no work
                return Ok("Wait for the admin to accept");
            }

            ItemInfoTM? itemInfoTM = await _marketTMService.GetItemInfoMarket(gameItem);

            if (itemInfoTM == null || itemInfoTM.Offers!.Count <= 0) return NotFound("Item no such in platform");

            decimal minItemPriceTM = decimal.Parse(itemInfoTM.MinPrice!) / 100;

            if(minItemPriceTM > gameItem.GameItemCost * 1.1M) return Forbid("Item no stability price, exchange");

            ItemBuyTM? itemBuyTM = await _marketTMService.BuyItemMarket(gameItem, 
                withdrawItem.SteamTradePartner!, withdrawItem.SteamTradeToken!);

            if(itemBuyTM is null) return NotFound("Trade url is incorrect");

            context.UserInventory.Remove(userInventory);

            return Ok();
        }

        [Authorize]
        [HttpGet("hmac")]
        public async Task<IActionResult> CreateHMAC()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string hash = 
                "project:[project];" +
                "user:[user];" +
                "currency:[currency];" +
                "success_url:[success_url];" +
                "fail_url:[fail_url]";

            string hmac = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return Ok(hmac);
        }

        [AllowAnonymous]
        [HttpPost("confirm/deposit")] 
        public async Task<IActionResult> TopUpBalanceConfirm(PaymentAnswerPattern paymentAnswer)
        {
            if(paymentAnswer.StatusAnswer != "success") return BadRequest(paymentAnswer.ParametersAnswer);

            string hash = 
                "project:[project];" + 
                "user:[user];" + 
                "currency:[currency];" + 
                "success_url:[success_url];" + 
                "fail_url:[fail_url]";

            byte[] hashOfDataInSignIn = Encoding.ASCII.GetBytes(hash);
            byte[] signature = Encoding.ASCII.GetBytes(paymentAnswer.SignatureRSA!);

            if (!_rsaService.VerifySignature(hashOfDataInSignIn, signature)) return Forbid("Poshel hacker lesom");

            ResponseInvoiceStatusPattern? invoiceInfoStatus = await _gameMoneyService
                .GetInvoiceStatusInfo(paymentAnswer.Invoice);
            if (invoiceInfoStatus is null || invoiceInfoStatus.Status != "Paid") return Forbid("Some times");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            
            UserAdditionalInfo info = (await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == invoiceInfoStatus.UserId))!;
            info.UserBalance += invoiceInfoStatus.Amount; //TODO Transfer for coin [RUB/UST/BTC/etc] 1/7

            await context.SaveChangesAsync();
            
            return Ok();
        }

        [Authorize]
        [HttpPut("exchange/{id}")]
        public async Task<IActionResult> ExchangeGameItem(GameItem gameItem, Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? searchInventory = await context.UserInventory
                .Include(x => x.GameItem)
                .FirstOrDefaultAsync(x => x.UserId == UserId && x.Id == id);
            GameItem? searchGameItem = await context.GameItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == gameItem.Id);

            if (searchInventory == null) return NotFound();
            if (searchGameItem == null) return NotFound();
            decimal differenceCost = searchInventory.GameItem!.GameItemCost - searchGameItem.GameItemCost;
            if (differenceCost < 0) return Forbid();

            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            if (info == null) return NotFound();

            searchInventory.GameItemId = searchGameItem.Id;
            info.UserBalance += differenceCost;

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
