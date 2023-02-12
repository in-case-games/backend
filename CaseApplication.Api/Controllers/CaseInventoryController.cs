using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
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
                .Include(x => x.GameCase)
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
                .Include(x => x.GameCase)
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
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == caseId)
                .ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(CaseInventoryDto caseInventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper? mapper = _mapperConfiguration.CreateMapper();
            CaseInventory caseInventory = mapper.Map<CaseInventory>(caseInventoryDto);

            await context.CaseInventory.AddAsync(caseInventory);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(CaseInventoryDto caseInventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? oldCaseInventory = await context.CaseInventory
                .FirstOrDefaultAsync(x => x.Id == caseInventoryDto.Id);

            if (oldCaseInventory is null)
                return NotFound("There is no such case inventory in the database, " +
                    "review what data comes from the api");
            
            IMapper? mapper = _mapperConfiguration.CreateMapper();
            CaseInventory newCaseInventory = mapper.Map<CaseInventory>(caseInventoryDto);

            context.Entry(oldCaseInventory).CurrentValues.SetValues(newCaseInventory);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? searchCaseInventory = await context
                .CaseInventory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchCaseInventory is null)
                return NotFound("There is no such case inventory in the database, " +
                    "review what data comes from the api");

            context.CaseInventory.Remove(searchCaseInventory);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
