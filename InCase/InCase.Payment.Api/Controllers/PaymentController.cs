﻿using InCase.Domain.Common;
using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace InCase.Payment.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private const decimal CostInCoin = 7M;
        private const decimal UpperLimitCost = 1.1M;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly WithdrawItemService _withdrawService;
        private readonly EncryptorService _rsaService;
        private readonly GameMoneyService _gameMoneyService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            WithdrawItemService withdrawService,
            EncryptorService rsaService,
            GameMoneyService gameMoneyService)
        {
            _contextFactory = contextFactory;
            _withdrawService = withdrawService;
            _rsaService = rsaService;
            _gameMoneyService = gameMoneyService;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawItem(DataWithdrawItem data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? inventory = await context.UserInventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game!)
                    .ThenInclude(g => g.Markets)
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == data.InventoryId && ui.UserId == UserId);

            if (inventory == null)
                return ResponseUtil.NotFound("Предмет не найден в инвенторе");

            GameItem item = inventory.Item!;

            ItemInfo? itemInfo = await _withdrawService.GetItemInfo(item);

            if (itemInfo is null || itemInfo.Result != "ok")
                return ResponseUtil.RequestTimeout("Сервис покупки предмета не отвечает");

            decimal itemPrice = itemInfo.PriceKopecks * 0.01M;

            if (itemPrice > item.Cost * UpperLimitCost / CostInCoin)
                return ResponseUtil.Conflict("Цена на предмет нестабильна");

            BalanceMarket balance = await _withdrawService.GetBalance(itemInfo.Market.Name!);

            if (balance.Result != "ok")
                return ResponseUtil.RequestTimeout("Сервис покупки предмета не отвечает");
            if (balance.Balance <= itemPrice) 
                return ResponseUtil.PaymentRequired("Ожидаем пополнения сервиса покупки");

            BuyItem buyItem = await _withdrawService.BuyItem(itemInfo, data.TradeUrl!);

            if (buyItem.Result != "ok")
                return ResponseUtil.RequestTimeout("Сервис покупки предмета не отвечает");

            ItemWithdrawStatus status = await context.ItemWithdrawStatuses
                .AsNoTracking()
                .FirstAsync(iws => iws.Name == "purchase");

            UserHistoryWithdraw withdraw = new()
            {
                IdForMarket = buyItem.Id,
                StatusId = status.Id,
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = UserId,
                MarketId = buyItem.Market!.Id,
                FixedCost = inventory.FixedCost
            };

            await context.UserHistoryWithdraws.AddAsync(withdraw);
            context.UserInventories.Remove(inventory);

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(withdraw.Convert(false));
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("deposit/signature")]
        public async Task<IActionResult> GetSignatureForDeposit()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string hash = _gameMoneyService.CreateHashOfDataForDeposit(UserId);
            string hmac = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return ResponseUtil.Ok(new { hmac });
        }

        [AllowAnonymous]
        [HttpPost("deposit")]
        public async Task<IActionResult> TopUpBalance(ResponsePaymentGM answer)
        {
            if (answer.StatusAnswer != "success")
                return ResponseUtil.Forbidden("Ожидаем оплаты");

            byte[] hash = Encoding.ASCII.GetBytes(answer.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(answer.SignatureRSA!);

            if (!_rsaService.VerifySignatureRSA(hash, signature))
                return ResponseUtil.Forbidden("Неверная подпись rsa");

            ResponseInvoiceStatusGM? invoice = await _gameMoneyService
                .GetInvoiceStatusInfo(answer.Invoice!);

            if (invoice is null)
                return ResponseUtil.Ok("Подождите некоторое время для пополнения");

            signature = Encoding.ASCII.GetBytes(invoice.SignatureRSA!);
            hash = Encoding.ASCII.GetBytes(invoice.ToString()!);

            if (!_rsaService.VerifySignatureRSA(hash, signature))
                return ResponseUtil.Forbidden("Неверная подпись rsa");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string nameStatus = invoice.Status!.Replace("_", "-").ToLower();

            InvoicePaymentStatus status = await context.InvoicePaymentStatuses
                .AsNoTracking()
                .FirstAsync(ips => ips.Name == nameStatus);

            UserHistoryPayment payment = new()
            {
                Amount = invoice.Amount,
                Currency = invoice.CurrencyProject,
                Date = DateTime.Today.AddSeconds(answer.SendTimeAnswer),
                InvoiceId = invoice.InvoiceId,
                Rate = invoice.Rate,
                StatusId = status.Id,
                UserId = invoice.UserId
            };

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(payment.Convert(false));
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("paygate/balance/{currency}")]
        public async Task<IActionResult> GetPaygateBalance(string currency)
        {
            ResponseBalanceGM? balance = await _gameMoneyService.GetBalance(currency);

            return balance is null ?
                ResponseUtil.RequestTimeout("Game Money сервис не отвечает") : 
                ResponseUtil.Ok(balance);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("market/balance")]
        public async Task<IActionResult> GetMarketBalance(string name)
        {
            BalanceMarket balance = await _withdrawService.GetBalance(name);

            return balance.Result == "ok" ?
                ResponseUtil.Ok(balance.Balance) : 
                ResponseUtil.RequestTimeout("Сервис покупки предметов не отвечает");
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("withdraw/{id}/status")]
        public async Task<IActionResult> GetWithdrawStatus(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryWithdraw? withdraw = await context.UserHistoryWithdraws
                .Include(uhw => uhw.Item)
                .Include(uhw => uhw.Item!.Game)
                .Include(uhw => uhw.Market)
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhw => uhw.Id == id);

            if (withdraw is null)
                return ResponseUtil.NotFound("История вывода не найдена");

            TradeInfo info = await _withdrawService.GetTradeInfo(withdraw);

            return info.Result == "ok" ? 
                ResponseUtil.Ok(info) :
                ResponseUtil.RequestTimeout("Сервис покупки предметов не отвечает");
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetItemInfo(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItems
                .Include(gi => gi.Game!)
                    .ThenInclude(g => g.Markets)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id);

            if(item is null)
                return ResponseUtil.NotFound("Предмет не найден");

            ItemInfo? info = await _withdrawService.GetItemInfo(item);

            return info is null ?
                ResponseUtil.RequestTimeout("Сервис покупки предметов не отвечает") : 
                ResponseUtil.Ok(info);
        }
    }
}
