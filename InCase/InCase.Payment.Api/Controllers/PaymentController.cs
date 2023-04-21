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
                .Include(i => i.Item)
                .Include(i => i.Item!.Game!)
                    .ThenInclude(ti => ti.Markets)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == data.InventoryId && f.UserId == UserId);

            if (inventory == null)
                return ResponseUtil.NotFound(nameof(UserInventory));

            GameItem item = inventory.Item!;

            ItemInfo? itemInfo = await _withdrawService.GetItemInfo(item);

            if (itemInfo is null)
                return ResponseUtil.Conflict(nameof(ItemInfo));

            decimal itemInfoPrice = itemInfo.PriceKopecks * 0.01M;

            if (itemInfoPrice > item.Cost * UpperLimitCost / CostInCoin)
                return ResponseUtil.Conflict("Item no stability price, exchange");

            decimal balance = await _withdrawService.GetBalance(itemInfo.Market.Name!);

            if (balance <= itemInfoPrice) 
                return ResponseUtil.Conflict("Wait payment");

            BuyItem buyItem = await _withdrawService.BuyItem(itemInfo, data.TradeUrl!);

            if (buyItem.Result != "ok")
                return ResponseUtil.Conflict(nameof(BuyItem));

            ItemWithdrawStatus status = await context.ItemWithdrawStatuses
                .AsNoTracking()
                .FirstAsync(f => f.Name == "purchase");

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

            return ResponseUtil.Ok(withdraw);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("deposit/signature")]
        public async Task<IActionResult> GetSignatureForDeposit()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            string hash = _gameMoneyService.CreateHashOfDataForDeposit(UserId);
            string hmac = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return ResponseUtil.Ok(hmac);
        }

        [AllowAnonymous]
        [HttpPost("deposit")]
        public async Task<IActionResult> TopUpBalance(ResponsePaymentGM answer)
        {
            if (answer.StatusAnswer != "success")
                return ResponseUtil.Conflict(answer.ParametersAnswer ?? "Error");

            byte[] hashOfDataInSignIn = Encoding.ASCII.GetBytes(answer.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(answer.SignatureRSA!);

            if (!_rsaService.VerifySignatureRSA(hashOfDataInSignIn, signature))
                return Forbid("No verify signature rsa");

            ResponseInvoiceStatusGM? invoiceInfoStatus = await _gameMoneyService
                .GetInvoiceStatusInfo(answer.Invoice);

            if (invoiceInfoStatus is null || invoiceInfoStatus.Status != "Paid")
                return ResponseUtil.Ok("Wait some time for replenishment");

            byte[] signatureInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.SignatureRSA!);
            byte[] hashOfDataInvoice = Encoding.ASCII.GetBytes(invoiceInfoStatus.ToString()!);

            if (!_rsaService.VerifySignatureRSA(hashOfDataInvoice, signatureInvoice))
                return Forbid("No verify signature rsa");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo info = (await context.UserAdditionalInfos
                .FirstOrDefaultAsync(x => x.UserId == invoiceInfoStatus.UserId))!;

            info.Balance += invoiceInfoStatus.Amount * CostInCoin; //TODO Check current answer

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(info);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("balance/{currency}")]
        public async Task<IActionResult> GetTerminalBalance(string currency)
        {
            ResponseBalanceGM? balance = await _gameMoneyService.GetBalance(currency);

            return balance is null ?
                ResponseUtil.NotFound(nameof(ResponseBalanceGM)) : 
                ResponseUtil.Ok(balance);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("market/balance")]
        public async Task<IActionResult> GetMarketBalance(string name)
        {
            return ResponseUtil.Ok(await _withdrawService.GetBalance(name));
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("withdraw/status")]
        public async Task<IActionResult> GetWithdrawStatus(UserHistoryWithdraw withdraw)
        {
            return ResponseUtil.Ok(await _withdrawService.GetTradeInfo(withdraw));
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetItemInfo(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItems
                .Include(i => i.Game!)
                    .ThenInclude(ti => ti.Markets)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if(item is null)
                return ResponseUtil.NotFound(nameof(item));

            return ResponseUtil.Ok(await _withdrawService.GetItemInfo(item));
        }
    }
}
