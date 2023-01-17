namespace CaseApplication.DomainLayer.Entities
{
    public class UserInventory : BaseEntity
    {
        public User? User { get; set; }
        public GameItem? GameItem { get; set; }
    }
}
