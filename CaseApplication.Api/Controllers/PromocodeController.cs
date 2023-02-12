using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PromocodeController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("use/{name}")]
        public async Task<IActionResult> UsePromocode(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocode
                .AsNoTracking()
                .Include(x => x.PromocodeType)
                .FirstOrDefaultAsync(x => x.PromocodeName == name);

            if (promocode == null) return NotFound();

            List<PromocodesUsedByUser> promocodesUses = await context.PromocodeUsedByUsers
                     .AsNoTracking()
                     .Include(x => x.Promocode)
                     .Where(x => x.UserId == UserId)
                     .ToListAsync();

            bool isExistPromocode = promocodesUses.Exists(x => x.PromocodeId == promocode.Id);

            if (isExistPromocode)
                return UnprocessableEntity("Promocode is used");

            PromocodesUsedByUser promocodesUsedByUser = new PromocodesUsedByUser();

            promocodesUsedByUser.Id = new Guid();
            promocodesUsedByUser.UserId = UserId;
            promocodesUsedByUser.PromocodeId = promocode.Id;

            await context.PromocodeUsedByUsers.AddAsync(promocodesUsedByUser);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocode
                .AsNoTracking()
                .Include(x => x.PromocodeType)
                .FirstOrDefaultAsync(x => x.PromocodeName == name);

            return promocode is null ? NotFound() : Ok(promocode);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(Promocode promocode)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            promocode.Id = new Guid();

            await context.Promocode.AddAsync(promocode);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(Promocode promocode)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? oldPromocode = await context.Promocode
                .FirstOrDefaultAsync(x => x.Id == promocode.Id);

            if (oldPromocode is null)
                return NotFound("There is no such promocode in the database, " +
                    "review what data comes from the api");

            context.Entry(oldPromocode).CurrentValues.SetValues(promocode);

            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? searchPromocode = await context
                .Promocode
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchPromocode is null)
                return NotFound("There is no such promocode in the database, " +
                    "review what data comes from the api");

            context.Promocode.Remove(searchPromocode);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}