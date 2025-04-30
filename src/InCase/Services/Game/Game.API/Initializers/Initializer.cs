using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Game.API.Initializers;
public sealed class Initializer(IServiceProvider serviceProvider)
{
    private static bool IsEnd = false; 
    private static bool IsEndGameItem = false;
    private static bool IsEndLootBox = false;
    private static bool IsEndInventory = false;
    
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
        InitializeGameItems(context);
        InitializeLootBoxes(context);
        InitializeLootBoxInventories(context);

        IsEnd = IsEndGameItem && IsEndLootBox && IsEndInventory;
	}

    private static void InitializeGameItems(ApplicationDbContext context) {
        if (IsEndGameItem) return;

        Console.WriteLine("InitializeGameItems Start");

        var gameItemsDb = context.GameItems.ToList();
        var gameItems = Constants.SeedData.GameItems.All
            .Select(gi => 
                new GameItem() {
                    Id = gi.Id,
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
        if (IsEndLootBox) return;

        Console.WriteLine("InitializeLootBoxes Start");

        var boxesDb = context.LootBoxes.ToList();
        var boxes = Constants.SeedData.LootBoxes.All
            .Select(lb => 
                new LootBox() {
                    Id = lb.Id,
                    Cost = lb.Cost,
                    IsLocked = lb.IsLocked,
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
}