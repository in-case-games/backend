namespace Infrastructure.MassTransit.User
{
    public class UserAdditionalInfoTemplate : BaseTemplate
    {
        public DateTime? DeletionDate { get; set; }
        public string? RoleName { get; set; }
        public Guid UserId { get; set; }
    }
}
