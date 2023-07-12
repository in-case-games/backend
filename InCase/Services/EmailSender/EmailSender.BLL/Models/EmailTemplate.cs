namespace EmailSender.BLL.Models
{
    public class EmailTemplate
    {
        public string HeaderTitle { get; set; } = string.Empty;
        public string HeaderSubtitle { get; set; } = string.Empty;
        public string BodyTitle { get; set; } = string.Empty;
        public string BodyDescription { get; set; } = string.Empty;
        public string? BodyButtonText { get; set; }
        public string? BodyButtonLink { get; set; }

        public List<BannerTemplate> BannerTemplates { get; set; } = new();
    }
}
