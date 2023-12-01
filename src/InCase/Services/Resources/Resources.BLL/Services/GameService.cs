﻿using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Helpers;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GameResponse>> GetAsync(CancellationToken cancellation = default)
        {
            List<Game> games = await _context.Games
                .Include(g => g.Boxes!)
                    .ThenInclude(lb => lb.Inventories!)
                        .ThenInclude(lb => lb.Item)
                .AsNoTracking()
                .ToListAsync(cancellation);

            return games.ToResponse();
        }

        public async Task<GameResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            Game game = await _context.Games
                .Include(g => g.Boxes!)
                    .ThenInclude(lb => lb.Inventories!)
                        .ThenInclude(lb => lb.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellation) ?? 
                throw new NotFoundException("Игра не найдена");

            return game.ToResponse();
        }

        public async Task<GameResponse> GetAsync(string name, CancellationToken cancellation = default)
        {
            Game game = await _context.Games
                .Include(g => g.Boxes!)
                    .ThenInclude(lb => lb.Inventories!)
                        .ThenInclude(lb => lb.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == name, cancellation) ??
                throw new NotFoundException("Игра не найдена");

            return game.ToResponse();
        }
    }
}
