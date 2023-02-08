namespace CaseApplication.Api.Models
{
    public class EmailModel
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        public string UserToken { get; set; } = null!;
        public string UserIp { get; set; } = null!;
    }
}
