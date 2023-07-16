using Authentication.DAL.Entities;

namespace Authentication.BLL.Models
{
    public class UserRestrictionResponse : BaseEntity
    {
        public DateTime ExpirationDate { get; set; }

        public Guid UserId { get; set; }
    }
}
