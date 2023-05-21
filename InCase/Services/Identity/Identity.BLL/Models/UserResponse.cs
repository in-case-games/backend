namespace Identity.BLL.Models
{
    public class UserResponse
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public decimal Balance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
    }
}