using AutoMapper;
using CaseApplication.Domain.Dtos;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class CaseInventoryController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<CaseInventoryDto, CaseInventory>();
        });

        public CaseInventoryController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? caseInventory = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .FirstOrDefaultAsync(x => x.Id == id);

            return caseInventory is null ? NotFound(): Ok(caseInventory);
        }

        [AllowAnonymous]
        [HttpGet("ids/{caseId}&{itemId}")]
        public async Task<IActionResult> GetByIds(Guid caseId, Guid itemId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? caseInventory = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .FirstOrDefaultAsync(x => x.GameCaseId == caseId && x.GameItemId == itemId);

            return caseInventory is null ? NotFound() : Ok(caseInventory);
        }

        [AllowAnonymous]
        [HttpGet("all/{caseId}")]
        public async Task<IActionResult> GetAll(Guid caseId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == caseId)
                .ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(CaseInventoryDto inventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            CaseInventory inventory = mapper.Map<CaseInventory>(inventoryDto);
            inventory.Id = new Guid();

            await context.CaseInventory.AddAsync(inventory);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(CaseInventoryDto inventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? oldInventory = await context.CaseInventory
                .FirstOrDefaultAsync(x => x.Id == inventoryDto.Id);

            if (oldInventory is null) return NotFound();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            CaseInventory inventory = mapper.Map<CaseInventory>(inventoryDto);

            context.Entry(oldInventory).CurrentValues.SetValues(inventory);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? inventory = await context.CaseInventory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (inventory is null) return NotFound();

            context.CaseInventory.Remove(inventory);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
