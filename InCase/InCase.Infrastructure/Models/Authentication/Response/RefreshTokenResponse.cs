namespace InCase.Infrastructure.Models.Authentication.Response
{
    public class RefreshTokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAccess { get; set; }
        public DateTime ExpiresRefresh { get; set; }
    }
}
