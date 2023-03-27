namespace InCase.Domain.Entities.Resources
{
    public class News : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Content { get; set; }
    }
}
