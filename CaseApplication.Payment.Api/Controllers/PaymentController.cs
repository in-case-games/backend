using CaseApplication.Domain.Entities.Payment;
using CaseApplication.Domain.Entities.Resources;
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
        private readonly TradeMarketService _marketTMService;
        private readonly RSAService _rsaService;
        private readonly GameMoneyService _gameMoneyService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            TradeMarketService marketTMService,
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
        public async Task<IActionResult> WithdrawItem(DataWithdrawItem withdrawItem)
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

            //Check info item in tm
            ItemInfoTM? itemInfoTM = await _marketTMService.GetMarketItemInfo(gameItem);

            if (itemInfoTM == null || itemInfoTM.Offers!.Count <= 0) 
                return NotFound(new { Error = "Item no such in platform", Success = false });

            decimal minItemPriceTM = decimal.Parse(itemInfoTM.MinPrice!) / 100;

            if(minItemPriceTM > (gameItem.GameItemCost / 7) * 1.1M) 
                return Forbid("Item no stability price, exchange");

            //Check balance tm
            decimal balanceTM = await _marketTMService.GetTradeMarketInfo();

            if (balanceTM <= gameItem.GameItemCost / 7) 
                return Forbid("Wait payment");

            await _gameMoneyService.TransferMoneyToTradeMarket(gameItem.GameItemCost / 7);

            //Buy item tm
            ResponseBuyItemTM? itemBuyTM = await _marketTMService.BuyMarketItem(
                gameItem, 
                withdrawItem.SteamTradePartner!, 
                withdrawItem.SteamTradeToken!);

            if(itemBuyTM is null) 
                return NotFound(new { Error = "Trade url is incorrect" , Success = false});

            context.UserInventory.Remove(userInventory);

            return Ok(new { Message = "Item was withdrawed", Success = true });
        }

        [Authorize]
        [HttpGet("deposit/signature")]
        public async Task<IActionResult> GetSignatureForDeposit()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string hash = _gameMoneyService.CreateHashOfDataForDeposit(UserId);
            string hmac = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return Ok(new { Data = hmac, Success = true });
        }

        [AllowAnonymous]
        [HttpPost("deposit")] 
        public async Task<IActionResult> TopUpBalance(ResponsePaymentGM paymentAnswer)
        {
            if(paymentAnswer.StatusAnswer != "success")
                return BadRequest(paymentAnswer.ParametersAnswer);

            byte[] hashOfDataInSignIn = Encoding.ASCII.GetBytes(paymentAnswer.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(paymentAnswer.SignatureRSA!);

            if (!_rsaService.VerifySignature(hashOfDataInSignIn, signature))
                return Forbid("Poshel hacker lesom");

            ResponseInvoiceStatusGM? invoiceInfoStatus = await _gameMoneyService
                .GetInvoiceStatusInfo(paymentAnswer.Invoice);

            if (invoiceInfoStatus is null || invoiceInfoStatus.Status != "Paid")
                return Accepted(new { Message = "Payed. Wait some time for replenishment", Success = true });

            byte[] signatureInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.SignatureRSA!);
            byte[] hashOfDataInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.ToString()!);

            if (!_rsaService.VerifySignature(hashOfDataInvoice, signatureInvoice)) 
                return Forbid("Poshel hacker lesom");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            
            UserAdditionalInfo info = (await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == invoiceInfoStatus.UserId))!;

            info.UserBalance += invoiceInfoStatus.Amount * 7; //TODO Check current answer

            await context.SaveChangesAsync();
            
            return Ok(new { Success = true });
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
            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            if (searchInventory == null || searchGameItem == null || info == null)
                return NotFound(new { Error = "Data was not found", Success = false });
            decimal differenceCost = searchInventory.GameItem!.GameItemCost - searchGameItem.GameItemCost;
            if (differenceCost < 0) return Forbid();

            searchInventory.GameItemId = searchGameItem.Id;
            info.UserBalance += differenceCost;

            await context.SaveChangesAsync();

            return Ok(new { Message = "Item was succesfully exchanged", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/gamemoney/balance/{currency}")]
        public async Task<IActionResult> GetGameMoneyBalance(string currency)
        {
            ResponseBalanceGM? answerBalanceInfoGM = await _gameMoneyService.GetBalanceInfo(currency);

            if (answerBalanceInfoGM is null)
                return NotFound(new { Error = "Data was not found", Success = false });

            return Ok(new { Data = answerBalanceInfoGM, Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/market/balance")]
        public async Task<IActionResult> GetTradeMarketBalance()
        {
            return Ok(await _marketTMService.GetTradeMarketInfo());
        }
    }
}
