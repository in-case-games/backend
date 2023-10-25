namespace Infrastructure.MassTransit.Resources
{
    public class LootBoxInventoryTemplate : BaseTemplate
    {
        public int ChanceWining { get; set; }
        public bool IsDeleted { get; set; }

        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }
    }
}
