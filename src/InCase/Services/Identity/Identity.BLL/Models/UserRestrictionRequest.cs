using Identity.DAL.Entities;

namespace Identity.BLL.Models;

public class UserRestrictionRequest : BaseEntity
{
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Description { get; set; }

    public Guid UserId { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid TypeId { get; set; }
}