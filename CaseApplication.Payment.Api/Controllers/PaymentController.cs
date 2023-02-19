using CaseApplication.Domain.Entities;
using CaseApplication.Infrastructure.Data;
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
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("withdrawn")]
        public async Task<IActionResult> WithdrawItems()
        {
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
    }
}
