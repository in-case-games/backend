﻿using InCase.Infrastructure.Exceptions;
using InCase.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.API.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GameController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<Game> games = await context.Games
                .Include(i => i.Boxes)
                .Include(i => i.Items)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ApiResult.OK(games);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            Game game = await context.Games
                .Include(i => i.Boxes)
                .Include(i => i.Items)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken) ?? 
                throw new NotFoundException("Игра не найдена");

            return ApiResult.OK(game);
        }
    }
}
