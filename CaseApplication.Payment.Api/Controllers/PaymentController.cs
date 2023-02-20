using CaseApplication.Domain.Entities.External;
using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace CaseApplication.Payment.Api.Controllers
{
    [Route("payment/api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly HttpClient _httpClient = new();
        private readonly IConfiguration _configuration;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("withdrawn")]
        public async Task<IActionResult> WithdrawItem(WithdrawItem withdrawItem)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? userInventory = await context.UserInventory
                .Include(c => c.GameItem)
                .FirstOrDefaultAsync(x => x.GameItemId == withdrawItem.GameItemId && x.UserId == UserId);

            if(userInventory == null) return NotFound();
            if (userInventory.GameItem!.GameItemIdForPlatform is null)
            {
                //TODO Notify admin by telegram auto withdrawn no work
                return Ok();
            }
            if (await InStockMarket(userInventory.GameItem!))
            {

            }



            return Ok();
        }

        [Authorize]
        [HttpPost("deposit")] 
        public async Task<IActionResult> TopUpBalance()
        {
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

        private async Task<bool> InStockMarket(GameItem gameItem)
        {
            Dictionary<string, string> requestUrls = new() {
                { 
                    "csgo",
                    $"https://market.csgo.com/api/ItemInfo/" +
                    $"{gameItem.GameItemIdForPlatform}/ru/?" +
                    $"key={_configuration["MarketTM:Secret"]}"
                },
                { 
                    "dota2", 
                    $"https://market.dota2.net/api/ItemInfo/" +
                    $"{gameItem.GameItemIdForPlatform}/ru/?" +
                    $"key={_configuration["MarketTM:Secret"]}"
                }
            };

            string requestUrl = requestUrls.FirstOrDefault(x => x.Key == gameItem.GameItemType).Value;

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            await response.Content
                .ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return true;
        }
    }
}
