namespace CaseApplication.DomainLayer.Entities
{
    public class UserInventory : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid GameItemId { get; set; }
        public GameItem? GameItem { get; set; }
    }
}
