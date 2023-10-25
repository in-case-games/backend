using Identity.DAL.Entities;

namespace Identity.BLL.Models
{
    public class UserAdditionalInfoResponse : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public UserRoleResponse? Role { get; set; }

        public Guid UserId { get; set; }
    }
}
