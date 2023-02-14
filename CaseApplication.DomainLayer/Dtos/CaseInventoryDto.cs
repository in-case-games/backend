using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    public class CaseInventoryDto: BaseEntity
    {
        public Guid GameCaseId { get; set; }
        public Guid GameItemId { get; set; }
        public int NumberItemsCase { get; set; }
        public int LossChance { get; set; }
    }
}
