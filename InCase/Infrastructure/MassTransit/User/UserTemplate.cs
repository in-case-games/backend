namespace Infrastructure.MassTransit.User
{
    public class UserTemplate : BaseTemplate
    {
        public string? Email { get; set; }
        public string? Login { get; set; }
        public bool IsDeleted { get; set; }
    }
}
