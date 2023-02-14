using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
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
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<PromocodeDto, Promocode>();
        });

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
                .FirstOrDefaultAsync(x => x.PromocodeName == name);

            if (promocode == null) return NotFound();

            bool isUsedPromocode = await context.PromocodeUsedByUsers.AnyAsync(
                x => x.PromocodeId == promocode.Id && x.UserId == UserId);

            if (isUsedPromocode) return UnprocessableEntity("Promocode is used");

            PromocodesUsedByUser promocodesUsedByUser = new() { 
                Id = new Guid(),
                UserId = UserId,
                PromocodeId = promocode.Id,
            };

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
        public async Task<IActionResult> Create(PromocodeDto promocodeDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            Promocode promocode = mapper.Map<Promocode>(promocodeDto);
            promocode.Id = new Guid();

            await context.Promocode.AddAsync(promocode);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(PromocodeDto promocodeDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? oldPromocode = await context.Promocode
                .FirstOrDefaultAsync(x => x.Id == promocodeDto.Id);

            if (oldPromocode is null) return NotFound();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            Promocode promocode = mapper.Map<Promocode>(promocodeDto);

            context.Entry(oldPromocode).CurrentValues.SetValues(promocode);

            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocode
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promocode is null) return NotFound();

            context.Promocode.Remove(promocode);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}