namespace Authentication.BLL.MassTransit.Models
{
    public class EmailBodyTemplate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ButtonText { get; set; }
        public string? ButtonLink { get; set; }
    }
}
