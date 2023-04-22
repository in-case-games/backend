using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;

namespace InCase.Infrastructure.Services
{
    public class WithdrawItemService
    {
        private readonly Dictionary<string, ITradeMarket> _tradeMarketServices;

        public WithdrawItemService(TradeMarketService tradeMarketService)
        {
            _tradeMarketServices = new()
            {
                ["tmcsgo"] = tradeMarketService,
                ["tmdota2"] = tradeMarketService,
                ["codashop"] = tradeMarketService,
            };
        }

        public async Task<BalanceMarket> GetBalance(string name)
        {
            bool IsExistService = _tradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            int numberAttempts = 5;

            BalanceMarket balance = new();

            while(numberAttempts > 0 && balance.Result != "ok")
            {
                balance = await _tradeMarketServices[name].GetBalance();

                numberAttempts--;
            }

            return balance;
        }

        public async Task<ItemInfo?> GetItemInfo(GameItem item)
        {
            Game game = item.Game ?? throw new ArgumentNullException("Game", 
                "Along with the item, it is necessary to transfer the game");

            List<GameMarket> markets = game.Markets ?? throw new ArgumentNullException("GameMarket",
                "Along with the game, it is necessary to transfer the markets");

            if (markets.Count == 0)
                throw new ArgumentException("The game has no ways to output an item");

            List<ItemInfo> itemInfos = new();
            ItemInfo itemInfo = new();
            int indexMarket = 0;

            while(indexMarket < markets.Count)
            {
                GameMarket market = markets[indexMarket];
                string name = market.Name!;

                int numberAttempts = 5;

                while (numberAttempts > 0 && itemInfo.Result != "ok")
                {
                    itemInfo = await _tradeMarketServices[name].GetItemInfo(item);

                    numberAttempts--;
                }

                if (itemInfo.Result == "ok" && itemInfo.Count > 0)
                {
                    itemInfo!.Market = market;
                    itemInfos.Add(itemInfo);
                }

                indexMarket++;
            }

            return itemInfos.MinBy(m => m.PriceKopecks);
        }

        public async Task<BuyItem> BuyItem(ItemInfo info, string tradeUrl)
        {
            string name = info.Market.Name ?? throw new ArgumentNullException("GameMarket",
                    "Along with the game, it is necessary to transfer the markets");

            bool IsExistService = _tradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            BuyItem? buyItem = new();
            int numberAttempts = 5;

            while(numberAttempts != 0 && buyItem.Result != "ok")
            {
                buyItem = await _tradeMarketServices[name].BuyItem(info, tradeUrl);
                buyItem.Market = info.Market;

                numberAttempts--;
            }

            return buyItem;
        }

        public async Task<TradeInfo> GetTradeInfo(UserHistoryWithdraw withdraw)
        {
            string name = withdraw.Market?.Name ?? throw new ArgumentNullException("GameMarket",
                    "Along with the game, it is necessary to transfer the markets");

            bool IsExistService = _tradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("Along with the game, it is necessary to transfer the markets");

            TradeInfo tradeInfo = new();
            int numberAttempts = 5;

            while(numberAttempts > 0 && tradeInfo.Result != "ok")
            {
                tradeInfo = await _tradeMarketServices[name].GetTradeInfo(withdraw);

                numberAttempts--;
            }

            return tradeInfo;
        }
    }
}
