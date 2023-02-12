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
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            gameCase!.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();

            if (gameCase != null)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;

                return Ok(gameCase);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking().FirstOrDefaultAsync(x => x.GameCaseName == name);

            gameCase!.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();

            if (gameCase != null)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;

                return Ok(gameCase);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking().ToListAsync();

            foreach (GameCase gameCase in gameCases)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;
            }

            return Ok(gameCases);
        }

        [AllowAnonymous]
        [HttpGet("groupname/{groupName}")]
        public async Task<IActionResult> GetAllByGroupName(string groupName)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking()
                .Where(x => x.GroupCasesName == groupName)
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
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            gameCase!.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();

            return gameCase is null ? NotFound(): Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(GameCaseDto gameCaseDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase gameCase = mapper.Map<GameCase>(gameCaseDto);

            await context.GameCase.AddAsync(gameCase);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(GameCase gameCase)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? oldCase = await context.GameCase
                .FirstOrDefaultAsync(x => x.Id == gameCase.Id);

            if (oldCase is null)
                return NotFound("There is no such case in the database, " +
                    "review what data comes from the api");

            context.Entry(oldCase).CurrentValues.SetValues(gameCase);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? searchCase = await context.GameCase
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchCase is null)
                return NotFound("There is no such case in the database, " +
                    "review what data comes from the api");

            context.GameCase.Remove(searchCase);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
