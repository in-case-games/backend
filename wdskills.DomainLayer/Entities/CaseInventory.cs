namespace wdskills.DomainLayer.Entities
{
    public class CaseInventory : BaseEntity
    {
        public GameCase? GameCase { get; set; }
        public GameItem? CaseItem { get; set; }
        public string? CaseImage { get; set; }
        public decimal? LossChance { get; set; }
    }
}
