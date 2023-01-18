namespace CaseApplication.DomainLayer.Entities
{
    public class UserInventory : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }
    }
}
