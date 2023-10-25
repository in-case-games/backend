using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.MassTransit;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWithdrawItemService _withdrawService;
        private readonly BasePublisher _publisher;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UserInventoryService(
            ApplicationDbContext context, 
            IWithdrawItemService withdrawService,
            BasePublisher publisher)
        {
            _context = context;
            _withdrawService = withdrawService;
            _publisher = publisher;
        }

        public async Task<List<UserInventoryResponse>> GetAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.Inventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<List<UserInventoryResponse>> GetAsync(Guid userId, int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.Inventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .OrderByDescending(ui => ui.Date)
                .Take(count)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<UserInventory?> GetByConsumerAsync(Guid id) => await _context.Inventories
            .AsNoTracking()
            .FirstOrDefaultAsync(ui => ui.Id == id);

        public async Task<UserInventoryResponse> GetByIdAsync(Guid id)
        {
            UserInventory inventory = await _context.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id) ?? 
                throw new NotFoundException("Инвентарь не найден");

            return inventory.ToResponse();
        }

        public async Task<UserInventoryResponse> CreateAsync(UserInventoryTemplate template)
        {
            if (!await _context.Items.AnyAsync(gi => gi.Id == template.ItemId))
                throw new NotFoundException("Предмет не найден");
            if (!await _context.Users.AnyAsync(u => u.Id == template.UserId))
                throw new NotFoundException("Пользователь не найден");

            UserInventory inventory = template.ToEntity();

            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            return inventory.ToResponse();
        }

        public async Task<List<UserInventoryResponse>> ExchangeAsync(ExchangeItemRequest request, Guid userId)
        {
            if (request.Items is null)
                throw new BadRequestException("Не выбран ни один предмет для обмена");

            if (request.Items.Sum(x => x.Count) > 10)
                throw new BadRequestException("Запрещено выводить более 10 предметов");

            UserInventory inventory = await _context.Inventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game!)
                    .ThenInclude(g => g.Market)
                .FirstOrDefaultAsync(ui => ui.Id == request.InventoryId && ui.UserId == userId) ??
                throw new NotFoundException("Заменяемый предмет не найден в инвентаре");

            ItemInfoResponse itemInfo = await _withdrawService
                .GetItemInfoAsync(inventory.Item!);

            decimal price = itemInfo.PriceKopecks * 0.01M;
            decimal itemCost = inventory.FixedCost / 7;

            if (price >= itemCost * 0.9M && price <= itemCost * 1.1M)
                throw new ConflictException("Товар может быть обменен только в случае нестабильности цены");

            List<UserInventory> inventories = new();

            decimal totalItemsCost = 0;

            foreach (ExchangeItemModel itemModel in request.Items)
            {
                GameItem gameItem = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == itemModel.ItemId) ?? 
                throw new NotFoundException($"Предмет с заданным Id {itemModel.ItemId} не найден");

                totalItemsCost += gameItem.Cost * itemModel.Count;

                for (int i = 0; i < itemModel.Count; i++)
                {
                    inventories.Add(new() {
                        FixedCost = gameItem.Cost,
                        Date = inventory.Date,
                        ItemId = gameItem.Id,
                        UserId = userId
                    });
                }
            }

            decimal differenceCost = inventory.FixedCost - totalItemsCost;

            if (differenceCost < 0)
                throw new BadRequestException("Стоимость товара при обмене не может быть выше");

            _context.Inventories.Remove(inventory);
            await _context.Inventories.AddRangeAsync(inventories);

            await _context.SaveChangesAsync();

            foreach (UserInventory userInventory in inventories)
            {
                UserInventoryBackTemplate template = userInventory.ToTemplate();
                template.FixedCost = differenceCost;
                await _publisher.SendAsync(template);
            }
            
            logger.Log(NLog.LogLevel.Info, "Exchanged: UserId - {0}," +
                                           " GameItemId - {1}, Game - {2}",
                                           inventory.UserId, inventory.ItemId,
                                           inventory.Item?.Game?.Name);

            return inventories.ToResponse();
        }
        public async Task<SellItemResponse> SellAsync(Guid id, Guid userId)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            UserInventory inventory = await _context.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id && ui.UserId == userId) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            logger.Log(NLog.LogLevel.Info, "Selled: UserId - {0}," +
                                           " GameItemId - {1}, Game - {2}",
                                           user.Id, inventory.ItemId,
                                           inventory.Item?.Game?.Name);

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(inventory.ToTemplate());

            return new() { Cost = inventory.FixedCost };
        }

        public async Task<SellItemResponse> SellLastAsync(Guid itemId, Guid userId)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(uai => uai.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.Inventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId && ui.ItemId == itemId)
                .ToListAsync();

            if (inventories.Count == 0)
                throw new ConflictException("Инвентарь пуст");

            UserInventory inventory = inventories.MinBy(ui => ui.Date)!;

            logger.Log(NLog.LogLevel.Info, "SelledLast: UserId - {0}," +
                                           " GameItemId - {1}, Game - {2}",
                                           user.Id, inventory.ItemId,
                                           inventory.Item?.Game?.Name);

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(inventory.ToTemplate());

            return new() { Cost = inventory.FixedCost };
        }

        public async Task<UserInventoryResponse> DeleteAsync(Guid id)
        {
            UserInventory inventory = await _context.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return inventory.ToResponse();
        }
    }
}
