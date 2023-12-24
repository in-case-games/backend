using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Infrastructure.MassTransit.Statistics;
using Microsoft.Extensions.Logging;
using Withdraw.BLL.MassTransit;

namespace Withdraw.BLL.Services
{
    public class WithdrawService : IWithdrawService
    {
        private readonly IWithdrawItemService _withdrawService;
        private readonly ILogger<WithdrawService> _logger;

        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public WithdrawService(
            IWithdrawItemService withdrawService,
            ILogger<WithdrawService> logger,
            ApplicationDbContext context, 
            BasePublisher publisher)
        {
            _context = context;
            _withdrawService = withdrawService;
            _logger = logger;
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
            var itemCost = inventory.FixedCost / 7;
            var info = await _withdrawService.GetItemInfoAsync(item, cancellation);
            var price = info.PriceKopecks * 0.01M;

            if (price > itemCost * 1.1M) throw new ConflictException("Цена на предмет нестабильна");

            var balance = await _withdrawService.GetBalanceAsync(info.Market.Name!, cancellation);

            if (balance.Balance <= price) throw new PaymentRequiredException("Ожидаем пополнения сервиса покупки");

            var status = await _context.Statuses
                .AsNoTracking()
                .FirstAsync(iws => iws.Name == "recorded", cancellation);

            var withdraw = new UserHistoryWithdraw
            {
                InvoiceId = "0",
                TradeUrl = request.TradeUrl,
                StatusId = status.Id,
                Date = DateTime.UtcNow - TimeSpan.FromSeconds(120),
                UpdateDate = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = userId,
                MarketId = item.Game!.Market!.Id,
                FixedCost = inventory.FixedCost
            };

            _logger.LogInformation($"UserId - {userId} оформил заявку на вывод - {item.Id}");

            _context.Inventories.Remove(inventory);
            await _context.Withdraws.AddAsync(withdraw, cancellation);
            await _context.SaveChangesAsync(cancellation);

            _logger.LogInformation($"UserId - {userId} сделал заявку на вывод - {item.Id}");

            await _publisher.SendAsync(new SiteStatisticsAdminTemplate { 
                FundsUsersInventories = -inventory.FixedCost 
            }, cancellation);

            return withdraw.ToResponse();
        }

        public async Task WithdrawStatusManagerAsync(CancellationToken cancellation = default)
        {
            var withdraw = await _context.Withdraws
                .Include(uhw => uhw.Item)
                .Include(uhw => uhw.Item!.Game)
                .Include(uhw => uhw.Market)
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .Where(uhw => 
                    uhw.UpdateDate + TimeSpan.FromSeconds(5) <= DateTime.UtcNow && 
                    (uhw.Date + TimeSpan.FromMinutes(5) > uhw.UpdateDate ||
                     uhw.UpdateDate + TimeSpan.FromMinutes(15) < DateTime.UtcNow) &&
                    uhw.Status!.Name != "given" && 
                    uhw.Status!.Name != "cancel" && 
                    uhw.Status!.Name != "blocked")
                .OrderByDescending(e => e.Date)
                .FirstOrDefaultAsync(cancellationToken: cancellation);

            if (withdraw is null) return;

            withdraw.UpdateDate = DateTime.UtcNow;
            _context.Entry(withdraw).Property(p => p.UpdateDate).IsModified = true;
            _context.Entry(withdraw).Property(p => p.StatusId).IsModified = true;
            _context.Entry(withdraw).Property(p => p.Date).IsModified = true;
            _context.Entry(withdraw).Property(p => p.InvoiceId).IsModified = true;

            var statuses = await _context.Statuses
                .AsNoTracking()
                .ToListAsync(cancellation);

            if (withdraw.Status!.Name == "recorded") await BuyItemAsync(withdraw, statuses, cancellation);
            else await UpdateStatusHistoryWithdraw(withdraw, statuses, cancellation);

            await _context.SaveChangesAsync(cancellation);
        }

        private async Task BuyItemAsync(UserHistoryWithdraw withdraw, IReadOnlyCollection<WithdrawStatus> statuses, CancellationToken cancellation)
        {
            var itemCost = withdraw.FixedCost / 7;
            var item = withdraw.Item!;
            item.Game!.Market = withdraw.Market;
            var info = await _withdrawService.GetItemInfoAsync(item, cancellation);

            if (info.PriceKopecks == 0)
            {
                _logger.LogError($"Не смог получить стоимость предмета - {item.Id}");
            }
            if (info.Count == 0)
            {
                _logger.LogError($"На площадке нет предметов на закупку - {item.Id}");
            }

            var price = info.PriceKopecks * 0.01M;

            _logger.LogInformation($"ItemId - {item.Id} Стоимость - {price}");

            if (price > itemCost * 1.1M || info.PriceKopecks <= 0 || info.Count <= 0)
            {
                withdraw.StatusId = statuses.First(s => s.Name == "cancel").Id;
                return;
            }

            var balance = await _withdrawService.GetBalanceAsync(info.Market.Name!, cancellation);

            if (balance.Balance <= price)
            {
                _logger.LogInformation("Ожидаем пополнения сервиса покупки");
                return;
            }

            _logger.LogInformation($"UserId - {withdraw.UserId}, ItemId - {withdraw.ItemId} отправил запрос на покупку;");

            withdraw.Date = DateTime.UtcNow - TimeSpan.FromSeconds(120);

            try
            {
                var buyItem = await _withdrawService.BuyItemAsync(info, withdraw.TradeUrl!, cancellation);

                withdraw.InvoiceId = buyItem.Id;
                withdraw.StatusId = statuses.First(ws => ws.Name == "purchase").Id;

                _logger.LogInformation($"UserId - {withdraw.UserId}, ItemId - {withdraw.ItemId} купил предмет;");
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError($"UserId - {withdraw.UserId}, ItemId - {withdraw.ItemId} поймал ошибку на покупке;");
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ex, ex.StackTrace);
            }
            catch (Exception ex)
            {
                //TODO Оповещение по тг
                _logger.LogCritical($"UserId - {withdraw.UserId}, ItemId - {withdraw.ItemId} поймал ошибку на покупке;");
                _logger.LogCritical($"UserId - {withdraw.UserId}, ItemId - {withdraw.ItemId} заблокирует вывод предмета;");
                _logger.LogCritical(ex, ex.Message);
                _logger.LogCritical(ex, ex.StackTrace);

                withdraw.StatusId = statuses.First(ws => ws.Name == "blocked").Id;
            }
        }

        private async Task UpdateStatusHistoryWithdraw(UserHistoryWithdraw withdraw, IEnumerable<WithdrawStatus> statuses, 
            CancellationToken cancellation)
        {
            var info = await _withdrawService.GetTradeInfoAsync(withdraw, cancellation);

            withdraw.StatusId = statuses.First(ws => ws.Name == info.Status).Id;
            await _context.SaveChangesAsync(cancellation);

            if (info.Status != "given") return;

            await _publisher.SendAsync(new SiteStatisticsTemplate
            {
                WithdrawnItems = 1,
                WithdrawnFunds = Convert.ToInt32(withdraw.FixedCost)
            }, cancellation);
        }
    }
}
