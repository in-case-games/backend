using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    public class UserAdditionalInfoDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid UserRoleId { get; set; }

        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
        public bool IsConfirmedAccount { get; set; }
    }
}
