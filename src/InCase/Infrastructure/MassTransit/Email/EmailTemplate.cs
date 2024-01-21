namespace Infrastructure.MassTransit.Email;

public class EmailTemplate
{
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public bool IsRequiredMessage { get; set; } = false;
    public EmailHeaderTemplate Header { get; set; } = new();
    public EmailBodyTemplate Body { get; set; } = new();

    public List<EmailBannerTemplate> BannerTemplates { get; set; } = [];
}