namespace Authentication.BLL.Models;

public class TokensResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAccess { get; set; }
    public DateTime ExpiresRefresh { get; set; }
}