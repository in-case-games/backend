namespace InCase.Domain.Entities.Email
{
    public class EmailTemplate
    {
        public string HeaderTittle { get; set; } = string.Empty;
        public string HeaderSubTittle { get; set; } = string.Empty;
        public string BodyTittle { get; set; } = string.Empty;
        public string BodyDescription { get; set; } = string.Empty;
        public string BodyButtonLink { get; set; } = string.Empty;
    }
}
