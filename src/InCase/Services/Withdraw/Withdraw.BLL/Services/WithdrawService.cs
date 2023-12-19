using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Infrastructure.MassTransit.Statistics;
using Withdraw.BLL.MassTransit;

namespace Withdraw.BLL.Services
{
    public class WithdrawService : IWithdrawService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWithdrawItemService _withdrawService;
        private readonly BasePublisher _publisher;

        public WithdrawService(
            ApplicationDbContext context, 
            IWithdrawItemService withdrawService,
            BasePublisher publisher)
        {
            _context = context;
            _withdrawService = withdrawService;
            _publisher = publisher;
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(Guid id, CancellationToken cancellation = default)
        {
            var item = await _context.Items
                .Include(gi => gi.Game)
                .Include(gi => gi.Game!.Market)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ?? 
                throw new NotFoundException("Предмет не найден");

            return await _withdrawService.GetItemInfoAsync(item, cancellation);
        }

        public async Task<BalanceMarketResponse> GetMarketBalanceAsync(string marketName, CancellationToken cancellation = default) =>
            await _withdrawService.GetBalanceAsync(marketName, cancellation);

        public async Task WithdrawStatusManagerAsync(int count, CancellationToken cancellation = default)
        {
            var withdraws = await _context.Withdraws
                .Include(uhw => uhw.Item)
                .Include(uhw => uhw.Item!.Game)
                .Include(uhw => uhw.Market)
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .Where(uhw => uhw.Status!.Name != "given" && uhw.Status!.Name != "cancel")
                .Take(count)
                .ToListAsync(cancellation);

            var statuses = await _context.Statuses
                .AsNoTracking()
                .ToListAsync(cancellation);

            foreach(var withdraw in withdraws)
            {
                var response = await _withdrawService.GetTradeInfoAsync(withdraw, cancellation);

                withdraw.StatusId = statuses.First(ws => ws.Name == response.Status).Id;
                _context.Entry(withdraw).Property(p => p.StatusId).IsModified = true;
                await _context.SaveChangesAsync(cancellation);

                if (response.Status != "given") continue;

                await _publisher.SendAsync(new SiteStatisticsTemplate
                {
                    WithdrawnItems = 1,
                    WithdrawnFunds = Convert.ToInt32(withdraw.FixedCost)
                }, cancellation);
            }
        }

        public async Task<UserHistoryWithdrawResponse> WithdrawItemAsync(WithdrawItemRequest request, Guid userId,
            CancellationToken cancellation = default)
        {
            var inventory = await _context.Inventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game)
                .Include(ui => ui.Item!.Game!.Market)
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == request.InventoryId && 
                ui.UserId == userId, cancellation) ??
                throw new NotFoundException("Предмет не найден в инвентаре");

            var item = inventory.Item!;

            var info = await _withdrawService.GetItemInfoAsync(item, cancellation);

            var price = info.PriceKopecks * 0.01M;
            var itemCost = inventory.FixedCost / 7;

            if (price > itemCost * 1.1M) throw new ConflictException("Цена на предмет нестабильна");

            var balance = await _withdrawService.GetBalanceAsync(info.Market.Name!, cancellation);

            if (balance.Balance <= price) throw new PaymentRequiredException("Ожидаем пополнения сервиса покупки");

            var buyItem = await _withdrawService.BuyItemAsync(info, request.TradeUrl!,cancellation);

            var status = await _context.Statuses
                .AsNoTracking()
                .FirstAsync(iws => iws.Name == "purchase", cancellation);

            var withdraw = new UserHistoryWithdraw
            {
                InvoiceId = buyItem.Id,
                StatusId = status.Id,
                Date = DateTime.UtcNow - TimeSpan.FromSeconds(120),
                ItemId = item.Id,
                UserId = userId,
                MarketId = buyItem.Market!.Id,
                FixedCost = inventory.FixedCost
            };

            _context.Inventories.Remove(inventory);
            await _context.Withdraws.AddAsync(withdraw, cancellation);

            await _context.SaveChangesAsync(cancellation);

            await _publisher.SendAsync(new SiteStatisticsAdminTemplate { 
                FundsUsersInventories = -inventory.FixedCost 
            }, cancellation);

            return withdraw.ToResponse();
        }
    }
}
