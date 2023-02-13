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
    public class GameCaseController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<GameCaseDto, GameCase>();
        });

        public GameCaseController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameCase == null) return NotFound();

            gameCase.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();
            
            gameCase.RevenuePrecentage = 0;
            gameCase.GameCaseBalance = 0;
            
            return Ok(gameCase);
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameCaseName == name);

            if (gameCase == null) return NotFound();

            gameCase.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();
            
            gameCase.RevenuePrecentage = 0;
            gameCase.GameCaseBalance = 0;
            
            return Ok(gameCase);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking()
                .ToListAsync();

            foreach (GameCase gameCase in gameCases)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;
            }

            return Ok(gameCases);
        }

        [AllowAnonymous]
        [HttpGet("groupname/{name}")]
        public async Task<IActionResult> GetAllByGroupName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking()
                .Where(x => x.GroupCasesName == name)
                .ToListAsync();

            foreach (GameCase gameCase in gameCases)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;
            }

            return Ok(gameCases);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameCase is null) return NotFound();

            gameCase.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();

            return Ok(gameCase);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(GameCaseDto gameCaseDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            GameCase gameCase = mapper.Map<GameCase>(gameCaseDto);
            gameCase.Id = Guid.NewGuid();

            await context.GameCase.AddAsync(gameCase);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(GameCaseDto newCaseDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? oldCase = await context.GameCase
                .FirstOrDefaultAsync(x => x.Id == newCaseDto.Id);

            if (oldCase is null) return NotFound();

            IMapper mapper = _mapperConfiguration.CreateMapper();
            GameCase newCase = mapper.Map<GameCase>(newCaseDto);

            context.Entry(oldCase).CurrentValues.SetValues(newCase);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? searchCase = await context.GameCase
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchCase is null) return NotFound();

            context.GameCase.Remove(searchCase);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
