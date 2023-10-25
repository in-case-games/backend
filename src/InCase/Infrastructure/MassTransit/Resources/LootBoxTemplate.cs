namespace Infrastructure.MassTransit.Resources
{
    public class LootBoxTemplate : BaseTemplate
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }

        public Guid GameId { get; set; }
    }
}
