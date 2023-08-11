using Microsoft.Extensions.Configuration;
using Resources.BLL.Interfaces;

namespace Resources.BLL.Services
{
    public class GamePlatformSteamService : IGamePlatformService
    {
        private readonly IConfiguration _cfg;
        private readonly IResponseService _responseService;
        private readonly Dictionary<string, string> AppId = new()
        {
            ["csgo"] = "111",
            ["dota"] = "111"
        };

        public GamePlatformSteamService(IConfiguration cfg, IResponseService responseService)
        {
            _cfg = cfg;
            _responseService = responseService;
        }

        public Task<decimal> GetItemCostAsync(string hashName, string game)
        {
            throw new NotImplementedException();
        }
    }
}
