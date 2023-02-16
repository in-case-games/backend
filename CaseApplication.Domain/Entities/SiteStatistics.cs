namespace CaseApplication.Domain.Entities
{
    public class SiteStatistics: BaseEntity
    {
        public int UsersCount { get; set; }
        public decimal SiteBalance { get; set; }
        public int CasesOpened { get; set; }
        public int ItemWithdrawn { get; set; }
        public int ReviewsWriten { get; set; }
    }
}
