namespace InCase.Domain.Entities.Auth
{
    public class DataSendTokens
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAccess { get; set; }
        public DateTime ExpiresRefresh { get; set; }
    }
}
