using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;

namespace Withdraw.BLL.Services
{
    public class WithdrawService : IWithdrawService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWithdrawItemService _withdrawItemService;

        public WithdrawService(
            ApplicationDbContext context, 
            IWithdrawItemService withdrawItemService)
        {
            _context = context;
            _withdrawItemService = withdrawItemService;
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(Guid id)
        {
            GameItem item = await _context.GameItems
                .Include(gi => gi.Game)
                .Include(gi => gi.Game!.Market)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            return await _withdrawItemService.GetItemInfoAsync(item);
        }

        public async Task<decimal> GetMarketBalanceAsync(string marketName) =>
            await _withdrawItemService.GetBalanceAsync(marketName);

        public async Task WithdrawStatusManagerAsync(int count, CancellationToken cancellationToken)
        {
            List<UserHistoryWithdraw> withdraws = await _context.HistoryWithdraws
                .Include(uhw => uhw.Item)
                .Include(uhw => uhw.Item!.Game)
                .Include(uhw => uhw.Market)
                .Include(uhw => uhw.Status)
                .Where(uhw => uhw.Status!.Name != "given" && uhw.Status!.Name != "cancel")
                .Take(count)
                .ToListAsync(cancellationToken);

            List<WithdrawStatus> statuses = await _context.WithdrawStatuses
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach(var withdraw in withdraws)
            {
                try
                {
                    TradeInfoResponse response = await _withdrawItemService
                        .GetTradeInfoAsync(withdraw);

                    withdraw.Status = statuses.First(ws => ws.Name == response.Status);
                }
                catch(Exception)
                {

                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserHistoryWithdrawResponse> WithdrawItemAsync(
            WithdrawItemRequest request, 
            Guid userId)
        {
            UserInventory inventory = await _context.UserInventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game)
                .Include(ui => ui.Item!.Game!.Market)
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == request.InventoryId && 
                ui.UserId == userId) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            GameItem item = inventory.Item!;

            ItemInfoResponse info = await _withdrawItemService.GetItemInfoAsync(item);

            decimal price = info.PriceKopecks * 0.01M;

            if (price > item.Cost * 1.1M / 7)
                throw new ConflictException("Цена на предмет нестабильна");

            decimal balance = await _withdrawItemService.GetBalanceAsync(info.Market.Name!);

            if (balance <= price)
                throw new PaymentRequiredException("Ожидаем пополнения сервиса покупки");

            BuyItemResponse buyItem = await _withdrawItemService.BuyItemAsync(info, request.TradeUrl!);

            WithdrawStatus status = await _context.WithdrawStatuses
                .AsNoTracking()
                .FirstAsync(iws => iws.Name == "purchase");

            UserHistoryWithdraw withdraw = new()
            {
                InvoiceId = buyItem.Id,
                StatusId = status.Id,
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = userId,
                MarketId = buyItem.Market!.Id,
                FixedCost = inventory.FixedCost
            };

            _context.UserInventories.Remove(inventory);
            await _context.HistoryWithdraws.AddAsync(withdraw);

            await _context.SaveChangesAsync();

            return withdraw.ToResponse();
        }
    }
}
