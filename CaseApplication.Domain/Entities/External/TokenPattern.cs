namespace CaseApplication.Domain.Entities.External
{
    public class TokenPattern
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAccessIn { get; set; }
        public DateTime ExpiresRefreshIn { get; set; }
    }
}
