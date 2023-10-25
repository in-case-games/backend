using Identity.DAL.Entities;

namespace Identity.BLL.Models
{
    public class UserResponse : BaseEntity
    {
        public string? Login { get; set; }

        public List<UserRestrictionResponse>? Restrictions { get; set; }
        public List<UserRestrictionResponse>? OwnerRestrictions { get; set; }
        public UserAdditionalInfoResponse? AdditionalInfo { get; set; }
    }
}
