namespace Infrastructure.MassTransit.User;
public class UserRestrictionTemplate : BaseTemplate
{
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public Guid UserId { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid TypeId { get; set; }
}