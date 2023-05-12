using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;
using InCase.Infrastructure.CustomException;

namespace InCase.Infrastructure.Services
{
    /// <summary>
    /// class <c>WithdrawItemService</c> designed for purchases of items for exchange
    /// by given market service
    /// </summary>
    public class WithdrawItemService
    {
        private readonly Dictionary<string, ITradeMarket> _marketServices;

        public WithdrawItemService(TradeMarketService marketService)
        {
            _marketServices = new()
            {
                ["tm"] = marketService,
            };
        }

        public async Task<BalanceMarket> GetBalance(string name)
        {
            bool IsExist = _marketServices.ContainsKey(name);

            if (!IsExist)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            int attempts = 5;

            BalanceMarket balance = new();

            while(attempts > 0 && balance.Result != "ok")
            {
                balance = await _marketServices[name].GetBalance();

                attempts--;
            }

            return balance.Result == "ok" ? balance :
                throw new RequestTimeoutCodeException("Сервис покупки предмета не отвечает");
        }

        public async Task<ItemInfo> GetItemInfo(GameItem item)
        {
            Game game = item.Game ?? throw new ArgumentNullException("Game", 
                "Along with the item, it is necessary to transfer the game");

            List<GameMarket> markets = game.Markets ?? throw new ArgumentNullException("GameMarket",
                "Along with the game, it is necessary to transfer the markets");

            if (markets.Count == 0)
                throw new ArgumentException("The game has no ways to output an item");

            List<ItemInfo> infos = new();
            ItemInfo info = new();
            int i = 0;

            while(i < markets.Count)
            {
                GameMarket market = markets[i];
                string name = market.Name!;

                int attempts = 5;

                while (attempts > 0 && info.Result != "ok")
                {
                    info = await _marketServices[name].GetItemInfo(item);

                    attempts--;
                }

                if (info.Result == "ok" && info.Count > 0)
                {
                    info!.Market = market;
                    infos.Add(info);
                }

                i++;
            }

            return infos.MinBy(m => m.PriceKopecks) ?? 
                throw new RequestTimeoutCodeException("Сервис покупки предмета не отвечает");
        }

        public async Task<BuyItem> BuyItem(ItemInfo info, string tradeUrl)
        {
            string name = info.Market.Name ?? throw new ArgumentNullException("GameMarket",
                    "Along with the game, it is necessary to transfer the markets");

            bool IsExist = _marketServices.ContainsKey(name);

            if (!IsExist)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            BuyItem? item = new();
            int attempts = 5;

            while(attempts != 0 && item.Result != "ok")
            {
                item = await _marketServices[name].BuyItem(info, tradeUrl);
                item.Market = info.Market;

                attempts--;
            }

            return item.Result == "ok" ? item :
                throw new RequestTimeoutCodeException("Сервис покупки предмета не отвечает");
        }

        public async Task<TradeInfo> GetTradeInfo(UserHistoryWithdraw withdraw)
        {
            string name = withdraw.Market?.Name ?? throw new ArgumentNullException("GameMarket",
                    "Along with the game, it is necessary to transfer the markets");

            bool IsExist = _marketServices.ContainsKey(name);

            if (!IsExist)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            TradeInfo info = new();
            int attempts = 5;

            while(attempts > 0 && info.Result != "ok")
            {
                info = await _marketServices[name].GetTradeInfo(withdraw);

                attempts--;
            }

            return info.Result == "ok" ? info : 
                throw new RequestTimeoutCodeException("Сервис покупки предмета не отвечает");
        }
    }
}
