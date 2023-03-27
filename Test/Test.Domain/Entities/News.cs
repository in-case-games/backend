namespace Test.Domain.Entities
{
    public class News : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
    }
}
