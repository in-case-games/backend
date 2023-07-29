namespace Infrastructure.MassTransit.User
{
    public class UserAdditionalInfoTemplate : BaseTemplate
    {
        public DateTime? DeletionDate { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}
