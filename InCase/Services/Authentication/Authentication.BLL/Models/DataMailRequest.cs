namespace Authentication.BLL.Models
{
    public class DataMailRequest
    {
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Ip { get; set; } = string.Empty;
        public string Platforms { get; set; } = string.Empty;
    }
}
