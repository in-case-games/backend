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
                ["market"] = marketTMService,
            };
        }

        public async Task<decimal> GetBalance(GamePlatform platform)
        {
            return 0M;
        }

        public async Task<ItemInfo> GetItemInfo(GameItem item)
        {
            Game game = item.Game ?? throw new ArgumentNullException("Game", 
                "Along with the item, it is necessary to transfer the game");

            List<GamePlatform> platforms = game.Platforms ?? throw new ArgumentNullException("GamePlatform",
                "Along with the game, it is necessary to transfer the platforms");

            if (platforms.Count == 0)
                throw new ArgumentException("The game has no ways to output an item");

            ItemInfo? itemInfo = new();
            int indexPlatform = 0;

            while(itemInfo.Price <= 0 || indexPlatform < platforms.Count)
            {
                GamePlatform platform = platforms[indexPlatform];

                itemInfo = await TradeMarketServices[platform.Name!].GetItemInfo(item);
                itemInfo.Platform = platforms[indexPlatform];
                itemInfo.Item = item;

                indexPlatform++;
            }

            if (itemInfo.Price <= 0)
                throw new Exception("POSHLI VSE NAXYI SERVICE OTKIS");
            if (itemInfo.Count <= 0)
                throw new Exception("POSHLI VSE NAXYI PREDMETOV NET");

            return itemInfo;
        }

        public async Task BuyItem(ItemInfo info, string tradeUrl)
        {

        }
    }
}
