namespace EmailSender.BLL.Models
{
    public class EmailTemplate
    {
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public bool IsRequiredMessage { get; set; } = false;
        public EmailHeaderTemplate Header { get; set; } = new EmailHeaderTemplate();
        public EmailBodyTemplate Body { get; set; } = new EmailBodyTemplate();

        public List<BannerTemplate> BannerTemplates { get; set; } = new();
    }
}
