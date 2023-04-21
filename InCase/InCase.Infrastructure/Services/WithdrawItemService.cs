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
                ["tm"] = tradeMarketService,
                ["codashop"] = tradeMarketService,
            };
        }

        public async Task<decimal> GetBalance(GameMarket market)
        {
            string name = market.Name ?? throw new ArgumentNullException("GameMarket",
                    "Along with the game, it is necessary to transfer the markets");

            bool IsExistService = _tradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("None service");

            return await _tradeMarketServices[name].GetBalance();
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
            int indexMarket = 0;

            while(indexMarket < markets.Count)
            {
                GameMarket market = markets[indexMarket];
                string name = market.Name!;

                ItemInfo itemInfo = await _tradeMarketServices[name].GetItemInfo(item);

                if(itemInfo.PriceKopecks > 0 && itemInfo.Count > 0)
                {
                    itemInfo.Market = market;
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
                throw new ArgumentException("None service");

            BuyItem buyItem = new();
            int numberAttempts = 5;

            while(numberAttempts != 0 || buyItem.Result != "OK")
            {
                ItemInfo? getInfo = await GetItemInfo(info.Item);

                if (getInfo is not null)
                {
                    buyItem = await _tradeMarketServices[name].BuyItem(getInfo, tradeUrl);
                    buyItem.Market = info.Market;
                }

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
                throw new ArgumentException("None service");

            return await _tradeMarketServices[name].GetTradeInfo(withdraw);
        }
    }
}
