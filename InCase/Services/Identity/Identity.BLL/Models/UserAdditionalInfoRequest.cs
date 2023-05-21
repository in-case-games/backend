namespace Identity.BLL.Models
{
    public class UserAdditionalInfoRequest
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public bool IsNotifyEmail { get; set; } = false;
        public bool IsGuestMode { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeletionDate { get; set; }

        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}