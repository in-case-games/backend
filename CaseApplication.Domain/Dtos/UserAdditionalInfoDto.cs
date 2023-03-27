using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    public class UserAdditionalInfoDto: BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid UserRoleId { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
        public bool IsConfirmedAccount { get; set; }
    }
}
