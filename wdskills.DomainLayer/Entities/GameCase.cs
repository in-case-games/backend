namespace wdskills.DomainLayer.Entities
{
    public class GameCase : BaseEntity
    {
        public string? GameCaseName { get; set; }
        public decimal GameCaseCost { get; set; }
        public decimal RevenuePrecentage { get; set; } = 0.1M;
    }
}
