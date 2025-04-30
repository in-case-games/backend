using Resources.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Resources.DAL.Entities;

namespace Resources.API.Initializers;
public sealed class Initializer(IServiceProvider serviceProvider)
{
    private static bool IsEnd = false; 
    private static bool IsEndGame = false;
    private static bool IsEndQuality = false;
    private static bool IsEndType = false;
    private static bool IsEndRarity = false;
    private static bool IsEndGameItem = false;
    private static bool IsEndLootBox = false;
    private static bool IsEndInventory = false;
    private static bool IsEndGroup = false;
    private static bool IsEndLootBoxGroup = false;

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
        InitializeGameItemQualities(context);
        InitializeGameItemTypes(context);
        InitializeGameItemRarities(context);
        InitializeGameItems(context);
        InitializeLootBoxes(context);
        InitializeLootBoxInventories(context);
        InitializeGroups(context);
        InitializeLootBoxGroups(context);

        IsEnd = IsEndGame && IsEndQuality && IsEndType && 
                IsEndRarity && IsEndGameItem && IsEndLootBox && 
                IsEndInventory && IsEndGroup && IsEndLootBoxGroup;
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

    private static void InitializeGameItemQualities(ApplicationDbContext context) {
        if(IsEndQuality) return;

        Console.WriteLine("InitializeGameItemQualities Start");

        var qualitiesDb = context.GameItemQualities.ToList();
        var qualities = Constants.SeedData.GameItemQualities.All
            .Select(q =>
                new GameItemQuality() {
                    Id = q.Id,
                    Name = q.Name
                })
            .Where(q => !qualitiesDb.Any(qDb => qDb.Id == q.Id));

        var qualitiesCount = qualities.Count();
        Console.WriteLine($"InitializeGameItemQualities - {qualitiesCount}");

        if (qualitiesCount == 0) {
            Console.WriteLine("InitializeGameItemQualities End");
            IsEndQuality = true;
            return;
        }
        
        context.GameItemQualities.AddRange(qualities);
        context.SaveChanges();
    }

    private static void InitializeGameItemTypes(ApplicationDbContext context) {
        if(IsEndType) return;

        Console.WriteLine("InitializeGameItemTypes Start");

        var typesDb = context.GameItemTypes.ToList();
        var types = Constants.SeedData.GameItemTypes.All
            .Select(t => 
                new GameItemType() {
                    Id = t.Id,
                    Name = t.Name
                })
            .Where(t => !typesDb.Any(tDb => tDb.Id == t.Id));

        var typesCount = types.Count();
        Console.WriteLine($"InitializeGameItemTypes - {typesCount}");

        if (typesCount == 0) {
            Console.WriteLine("InitializeGameItemTypes End");
            IsEndType = true;
            return;
        }

        context.GameItemTypes.AddRange(types);
        context.SaveChanges();
    }

    private static void InitializeGameItemRarities(ApplicationDbContext context) {
        if(IsEndRarity) return;

        Console.WriteLine("InitializeGameItemRarities Start");

        var rarityDb = context.GameItemRarities.ToList();
        var rarities = Constants.SeedData.GameItemRarities.All
            .Select(r =>
                new GameItemRarity() {
                    Id = r.Id,
                    Name = r.Name
                })
            .Where(r => !rarityDb.Any(rDb => rDb.Id == r.Id));

        var raritiesCount = rarities.Count();
        Console.WriteLine($"InitializeGameItemRarities - {raritiesCount}");

        if (raritiesCount == 0) {
            Console.WriteLine("InitializeGameItemRarities End");
            IsEndRarity = true;
            return;
        }

        context.GameItemRarities.AddRange(rarities);
        context.SaveChanges();
    }

    private static void InitializeGameItems(ApplicationDbContext context) {
        if (IsEndGameItem || !IsEndQuality || !IsEndRarity || !IsEndGame || !IsEndType) return;

        Console.WriteLine("InitializeGameItems Start");

        var gameItemsDb = context.GameItems.ToList();
        var gameItems = Constants.SeedData.GameItems.All
            .Select(gi => 
                new GameItem() {
                    Id = gi.Id,
                    Name = gi.Name,
                    HashName = gi.HashName,
                    IdForMarket = gi.IdForMarket,
                    GameId = gi.GameId,
                    TypeId = gi.TypeId,
                    RarityId = gi.RarityId,
                    QualityId = gi.QualityId,
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

    private static void InitializeLootBoxes(ApplicationDbContext context) {
        if (IsEndLootBox || !IsEndGame) return;

        Console.WriteLine("InitializeLootBoxes Start");

        var boxesDb = context.LootBoxes.ToList();
        var boxes = Constants.SeedData.LootBoxes.All
            .Select(lb => 
                new LootBox() {
                    Id = lb.Id,
                    Name = lb.Name,
                    Cost = lb.Cost,
                    IsLocked = lb.IsLocked,
                    GameId = lb.GameId
                })
            .Where(lb => !boxesDb.Any(lbDb => lbDb.Id == lb.Id));

        var boxesCount = boxes.Count();
        Console.WriteLine($"InitializeLootBoxes - {boxesCount}");

        if (boxesCount == 0) {
            Console.WriteLine("InitializeLootBoxes End");
            IsEndLootBox = true;
            return;
        }
        
        context.LootBoxes.AddRange(boxes);
        context.SaveChanges();
    }

    private static void InitializeLootBoxInventories(ApplicationDbContext context) {
        if (IsEndInventory || !IsEndGameItem || !IsEndLootBox) return;

        Console.WriteLine("InitializeLootBoxInventories Start");

        var inventoriesDb = context.LootBoxInventories.ToList();
        var inventories = Constants.SeedData.LootBoxesInventories.All
            .SelectMany(lbi => 
                lbi.Select(lb => 
                    new LootBoxInventory() {
                        Id = lb.Id,
                        ChanceWining = lb.ChanceWining,
                        ItemId = lb.Item.Id,
                        BoxId = lb.Box.Id
                    }
                ))
            .Where(lbi => !inventoriesDb.Any(lbiDb => lbiDb.Id == lbi.Id));
        
        var inventoriesCount = inventories.Count();
        Console.WriteLine($"InitializeLootBoxInventories - {inventoriesCount}");

        if (inventoriesCount == 0) {
            Console.WriteLine("InitializeLootBoxInventories End");
            IsEndInventory = true;
            return;
        }
        
        context.LootBoxInventories.AddRange(inventories);
        context.SaveChanges();
    }

    private static void InitializeGroups(ApplicationDbContext context) {
        if (IsEndGroup) return;

        Console.WriteLine("InitializeGroups Start");

        var groupsDb = context.GroupLootBoxes.ToList();
        var groups = Constants.SeedData.GroupLootBoxes.All
            .Select(lb => 
                new GroupLootBox() {
                    Id = lb.Id,
                    Name = lb.Name
                })
            .Where(lb => !groupsDb.Any(lbDb => lbDb.Id == lb.Id));
        
        var groupsCount = groups.Count();
        Console.WriteLine($"InitializeGroups - {groupsCount}");

        if (groupsCount == 0) {
            Console.WriteLine("InitializeGroups End");
            IsEndGroup = true;
            return;
        }
        
        context.GroupLootBoxes.AddRange(groups);
        context.SaveChanges();
    }

    private static void InitializeLootBoxGroups(ApplicationDbContext context) {
        if (IsEndLootBoxGroup || !IsEndGroup || !IsEndGame || !IsEndLootBox) return;

        Console.WriteLine("InitializeLootBoxGroups Start");

        var groupsDb = context.LootBoxGroups.ToList();
        var groups = Constants.SeedData.LootBoxGroups.All
            .SelectMany(lbg => 
                lbg.Select(lb => 
                    new LootBoxGroup() {
                        Id = lb.Id,
                        BoxId = lb.BoxId,
                        GameId = lb.GameId,
                        GroupId = lb.GroupId
                    }
                ))
            .Where(lbg => !groupsDb.Any(lbgDb => lbgDb.Id == lbg.Id));
        
        var groupsCount = groups.Count();
        Console.WriteLine($"InitializeLootBoxGroups - {groupsCount}");

        if (groupsCount == 0) {
            Console.WriteLine("InitializeLootBoxGroups End");
            IsEndLootBoxGroup = true;
            return;
        }
        
        context.LootBoxGroups.AddRange(groups);
        context.SaveChanges();
    }
}