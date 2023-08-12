namespace Infrastructure.MassTransit.Resources
{
    public class LootBoxBackTemplate : BaseTemplate
    {
        public decimal Cost { get; set; }
        public bool IsLocked { get; set; }
    }
}
