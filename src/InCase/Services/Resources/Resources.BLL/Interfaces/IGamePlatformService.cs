namespace Resources.BLL.Interfaces
{
    public interface IGamePlatformService
    {
        public Task<decimal> GetItemCostAsync(string hashName, string game);
    }
}
