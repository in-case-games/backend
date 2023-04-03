using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        public UserController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<User> users = await context.Users
                .AsNoTracking()
                .Include(x => x.AdditionalInfo)
                .ToListAsync();

            users.ForEach(x =>
            {
                x.PasswordSalt = "denied";
                x.PasswordHash = "denied";
            });

            return ResponseUtil.Ok(users);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return ResponseUtil.NotFound(nameof(User));

            user.PasswordHash = null;
            user.PasswordSalt = null;

            return ResponseUtil.Ok(user);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/promocodes/{userId}")]
        public async Task<IActionResult> GetPromocodes(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryPromocode> promocodes = await context.UserHistoryPromocodes
                .AsNoTracking()
                .Include(x => x.Promocode)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(promocodes);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/withdrawns/{userId}")]
        public async Task<IActionResult> GetWithdrawns(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryWithdrawn> withdrawns = await context.UserHistoryWithdrawns
                .AsNoTracking()
                .Include(x => x.Item)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(withdrawns);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/openings/{userId}")]
        public async Task<IActionResult> GetOpenings(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpening> openings = await context.UserHistoryOpenings
                .AsNoTracking()
                .Include(x => x.Box)
                .Include(x => x.Item)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(openings);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("banners/{userId}")]
        public async Task<IActionResult> GetPathBanners(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserPathBanner> banners = await context.UserPathBanners
                .AsNoTracking()
                .Include(x => x.Banner)
                .Include(x => x.Item)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(banners);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory/{userId}")]
        public async Task<IActionResult> GetInventory(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventories
                .AsNoTracking()
                .Include(x => x.Item)
                .ToListAsync();

            return ResponseUtil.Ok(inventories);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/payments/{userId}")]
        public async Task<IActionResult> GetPayments(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryPayment> payments = await context.UserHistoryPayments
                .AsNoTracking()
                .Where(x => x.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(payments);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("promocodes")]
        public async Task<IActionResult> ActivatePromocode(UserHistoryPromocodeDto promocode)
        {
            if (promocode.UserId == UserId)
                return await EndpointUtil.Create(promocode.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("payments")]
        public async Task<IActionResult> CreditPayment(UserHistoryPaymentDto paymentDto)
        {
            if (paymentDto.UserId == UserId)
                return await EndpointUtil.Create(paymentDto.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("openings")]
        public async Task<IActionResult> CreateOpening(UserHistoryOpeningDto opening)
        {
            if (opening.UserId == UserId)
                return await EndpointUtil.Create(opening.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("banners")]
        public async Task<IActionResult> CreatePathBanner(UserPathBannerDto banner)
        {
            if (banner.UserId == UserId)
                return await EndpointUtil.Create(banner.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("withdrawns")]
        public async Task<IActionResult> CreateWithdrawn(UserHistoryWithdrawnDto withdrawn)
        {
            if (withdrawn.UserId == UserId)
                return await EndpointUtil.Create(withdrawn.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToInventory(UserInventoryDto inventory)
        {
            if (inventory.UserId == UserId)
                return await EndpointUtil.Create(inventory.Convert(), _contextFactory);

            return Forbid("Access denied");
        }
        [AuthorizeRoles(Roles.All)]
        [HttpDelete("items")]
        public async Task<IActionResult> RemoveItem()
        {
            return await EndpointUtil.Delete<UserInventory>(UserId, _contextFactory);
        }
    }
}
