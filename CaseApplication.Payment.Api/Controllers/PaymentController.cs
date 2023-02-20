using CaseApplication.Domain.Entities.External;
using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Payment.Api.Controllers
{
    [Route("payment/api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MarketTMService _marketTMService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            MarketTMService marketTMService)
        {
            _contextFactory = contextFactory;
            _marketTMService = marketTMService;
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

            if(userInventory == null) return NotFound();
            GameItem gameItem = userInventory.GameItem!;

            //TODO Check valid partner and token and id
            if (gameItem.GameItemIdForPlatform is null)
            {
                //TODO Notify admin by telegram auto withdrawn no work
                return Ok();
            }

            ItemInfoTM? itemInfoTM = await _marketTMService.GetItemInfoMarket(gameItem);

            if (itemInfoTM == null) return NotFound();

            decimal minItemPriceTM = decimal.Parse(itemInfoTM.MinPrice!) / 100;

            if(minItemPriceTM > gameItem.GameItemCost * 1.1M) return Forbid();

            await _marketTMService.BuyItemMarket(gameItem, 
                withdrawItem.SteamTradePartner!, withdrawItem.SteamTradeToken!);

            context.UserInventory.Remove(userInventory);

            return Ok();
        }

        [Authorize]
        [HttpPost("deposit")] 
        public async Task<IActionResult> TopUpBalance()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

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
