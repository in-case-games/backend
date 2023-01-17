namespace CaseApplication.DomainLayer.Entities
{
    public class CaseInventory : BaseEntity
    {
        public Guid GameCaseId { get; set; }
        public GameCase? GameCase { get; set; }
        public Guid CaseItemId { get; set; }
        public GameItem? CaseItem { get; set; }
        public decimal? LossChance { get; set; }
    }
}
