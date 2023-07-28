using Identity.DAL.Entities;

namespace Identity.BLL.Models
{
    public class UserAdditionalInfoRequest : BaseEntity
    {
        public string? ImageUri { get; set; } = "";
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }

        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}