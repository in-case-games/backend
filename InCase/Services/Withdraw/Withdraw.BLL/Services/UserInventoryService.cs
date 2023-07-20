using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWithdrawItemService _withdrawService;
        private readonly IConfiguration _cfg;
        private readonly IBus _bus;

        public UserInventoryService(
            ApplicationDbContext context, 
            IWithdrawItemService withdrawService,
            IConfiguration cfg,
            IBus bus)
        {
            _context = context;
            _withdrawService = withdrawService;
            _cfg = cfg;
            _bus = bus;
        }

        public async Task<List<UserInventoryResponse>> GetAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.UserInventories
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

            List<UserInventory> inventories = await _context.UserInventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .OrderByDescending(ui => ui.Date)
                .Take(count)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<UserInventory?> GetByConsumerAsync(Guid id) => await _context.UserInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(ui => ui.Id == id);

        public async Task<UserInventoryResponse> GetByIdAsync(Guid id)
        {
            UserInventory inventory = await _context.UserInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id) ?? 
                throw new NotFoundException("Инвентарь не найден");

            return inventory.ToResponse();
        }

        public async Task CreateAsync(UserInventoryTemplate template)
        {
            UserInventory inventory = template.ToEntity();

            await _context.UserInventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<UserInventoryResponse> ExchangeAsync(Guid id, Guid itemId, Guid userId)
        {
            UserInventory inventory = await _context.UserInventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game!)
                    .ThenInclude(g => g.Market)
                .FirstOrDefaultAsync(ui => ui.Id == id && ui.UserId == userId) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            GameItem item = await _context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == itemId) ??
                throw new NotFoundException("Предмет не найден");

            decimal differenceCost = inventory.FixedCost - item.Cost;

            if (differenceCost < 0)
                throw new BadRequestException("Стоимость товара при обмене не может быть выше");

            ItemInfoResponse itemInfo = await _withdrawService
                .GetItemInfoAsync(inventory.Item!);

            decimal itemPrice = itemInfo.PriceKopecks * 0.01M;

            if (itemPrice <= inventory.FixedCost * 1.1M / 7)
                throw new ConflictException("Товар может быть обменен только в случае нестабильности цены");

            //TODO Write logs

            inventory.ItemId = item.Id;
            inventory.FixedCost = item.Cost;

            await _context.SaveChangesAsync();

            UserInventoryTemplate template = inventory.ToTemplate();
            template.FixedCost = differenceCost;

            Uri uri = new(_cfg["MassTransit:Uri"] + "/user-inventory_sell");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(template);

            return inventory.ToResponse();

        }
        public async Task<SellItemResponse> SellAsync(Guid id, Guid userId)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            UserInventory inventory = await _context.UserInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id && ui.UserId == userId) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            //TODO Write logs

            _context.UserInventories.Remove(inventory);
            await _context.SaveChangesAsync();

            Uri uri = new(_cfg["MassTransit:Uri"] + "/user-inventory_sell");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(inventory.ToTemplate());

            return new() { Cost = inventory.FixedCost };
        }

        public async Task<SellItemResponse> SellLastAsync(Guid itemId, Guid userId)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(uai => uai.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.UserInventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId && ui.ItemId == itemId)
                .ToListAsync();

            if (inventories.Count == 0)
                throw new ConflictException("Инвентарь пуст");

            UserInventory inventory = inventories.MinBy(ui => ui.Date)!;

            //TODO Write logs

            _context.UserInventories.Remove(inventory);
            await _context.SaveChangesAsync();

            Uri uri = new(_cfg["MassTransit:Uri"] + "/user-inventory_sell");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(inventory.ToTemplate());

            return new() { Cost = inventory.FixedCost };
        }
    }
}
