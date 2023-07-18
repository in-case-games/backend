namespace Infrastructure.MassTransit.Resources
{
    public class GameItemTemplate : BaseTemplate
    {
        public string? Name { get; set; }
        public string? HashName { get; set; }
        public decimal Cost { get; set; }
        public bool IsDeleted { get; set; }

        public Guid GameId { get; set; }
        public Guid TypeId { get; set; }
        public Guid RarityId { get; set; }
        public Guid QualityId { get; set; }
    }
}
