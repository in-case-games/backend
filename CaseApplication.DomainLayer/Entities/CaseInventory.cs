namespace CaseApplication.DomainLayer.Entities
{
    public class CaseInventory : BaseEntity
    {
        public Guid GameCaseId { get; set; }
        public Guid GameItemId { get; set; }
        public decimal? LossChance { get; set; }
    }
}
