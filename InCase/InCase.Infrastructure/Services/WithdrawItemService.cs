using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;

namespace InCase.Infrastructure.Services
{
    public class WithdrawItemService
    {
        private readonly Dictionary<string, ITradeMarket> TradeMarketServices = new();

        public WithdrawItemService(TradeMarketService marketTMService)
        {
            TradeMarketServices = new()
            {
                ["tm"] = marketTMService,
                ["codashop"] = marketTMService,
            };
        }

        public async Task<decimal> GetBalance(GamePlatform platform)
        {
            string name = platform.Name!;

            bool IsExistService = TradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("None service");

            return await TradeMarketServices[name].GetBalance();
        }

        public async Task<ItemInfo> GetItemInfo(GameItem item)
        {
            Game game = item.Game ?? throw new ArgumentNullException("Game", 
                "Along with the item, it is necessary to transfer the game");

            List<GamePlatform> platforms = game.Platforms ?? throw new ArgumentNullException("GamePlatform",
                "Along with the game, it is necessary to transfer the platforms");

            if (platforms.Count == 0)
                throw new ArgumentException("The game has no ways to output an item");

            List<ItemInfo> itemInfos = new();
            int indexPlatform = 0;

            while(indexPlatform < platforms.Count)
            {
                GamePlatform platform = platforms[indexPlatform];
                string name = platform.Name!;

                ItemInfo itemInfo = await TradeMarketServices[name].GetItemInfo(item);

                if(itemInfo.Price > 0 && itemInfo.Count > 0)
                {
                    itemInfo.Platform = platform;
                    itemInfos.Add(itemInfo);
                }

                indexPlatform++;
            }

            if (itemInfos.Count == 0)
                throw new Exception("VSE OTKISLO");

            return itemInfos.MinBy(m => m.Price)!;
        }

        public async Task<BuyItem> BuyItem(ItemInfo info, string tradeUrl)
        {
            string name = info.Platform.Name!;

            bool IsExistService = TradeMarketServices.ContainsKey(name);

            if (!IsExistService)
                throw new ArgumentException("None service");

            BuyItem buyItem = new();
            int numberAttempts = 5;

            while(numberAttempts != 0 || buyItem.Result != "OK")
            {
                info = await GetItemInfo(info.Item);
                buyItem = await TradeMarketServices[name].BuyItem(info.Item, tradeUrl);
                numberAttempts--;
            }

            if (buyItem.Result != "OK")
                throw new Exception("Buy item no success");

            return buyItem;
        }
    }
}
