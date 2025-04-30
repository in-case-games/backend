using Withdraw.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Withdraw.DAL.Entities;

namespace Withdraw.API.Initializers;
public sealed class Initializer(IServiceProvider serviceProvider)
{
    private static bool IsEnd = false; 
    private static bool IsEndGame = false;
    private static bool IsEndGameItem = false;

    private readonly object _locker = new();
	private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void InitializeDb() {
        if (IsEnd) return;

        lock (_locker) {
            while (!IsEnd) {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();
                Seed(context);
            }
        }
    }

	private static void Seed(ApplicationDbContext context) {
        InitializeGames(context);
        InitializeGameItems(context);

        IsEnd = IsEndGame && IsEndGameItem;
	}

    private static void InitializeGames(ApplicationDbContext context) {
        if(IsEndGame) return;

        Console.WriteLine("InitializeGames Start");

        var gamesDb = context.Games.ToList();
        var games = Constants.SeedData.Games.All
            .Select(g => 
                new Game() {
                    Id = g.Id,
                    Name = g.Name
                })
            .Where(g => !gamesDb.Any(gd => gd.Id == g.Id));

        var gamesCount = games.Count();
        Console.WriteLine($"InitializeGames - {gamesCount}");

        if (gamesCount == 0) {
            Console.WriteLine("InitializeGames End");
            IsEndGame = true;
            return;
        }

        context.Games.AddRange(games);
        context.SaveChanges();
    }

    private static void InitializeGameItems(ApplicationDbContext context) {
        if (IsEndGameItem || !IsEndGame) return;

        Console.WriteLine("InitializeGameItems Start");

        var gameItemsDb = context.GameItems.ToList();
        var gameItems = Constants.SeedData.GameItems.All
            .Select(gi => 
                new GameItem() {
                    Id = gi.Id,
                    IdForMarket = gi.IdForMarket,
                    GameId = gi.GameId,
                    Cost = gi.Cost
                })
            .Where(gi => !gameItemsDb.Any(giDb => giDb.Id == gi.Id));

        var gameItemsCount = gameItems.Count();
        Console.WriteLine($"InitializeGameItems - {gameItemsCount}");

        if (gameItemsCount == 0) {
            Console.WriteLine("InitializeGameItems End");
            IsEndGameItem = true;
            return;
        }

        context.GameItems.AddRange(gameItems);
        context.SaveChanges();
    }
}