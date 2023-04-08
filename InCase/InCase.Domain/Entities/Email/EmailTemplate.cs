namespace InCase.Domain.Entities.Email
{
    public class EmailTemplate
    {
        public string HeaderTitle { get; set; } = string.Empty;
        public string HeaderSubtitle { get; set; } = string.Empty;
        public string BodyTitle { get; set; } = string.Empty;
        public string BodyDescription { get; set; } = string.Empty;
        public string BodyButtonLink { get; set; } = string.Empty;
    }
}
