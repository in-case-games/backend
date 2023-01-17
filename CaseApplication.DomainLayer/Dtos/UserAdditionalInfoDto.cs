using CaseApplication.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.DomainLayer.Dtos
{
    public class UserAdditionalInfoDto : BaseEntityDto
    {
        public Guid UserId { get; set; }
        public Guid UserRoleId { get; set; }
        public int UserAge { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
    }
}
