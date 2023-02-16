using CaseApplication.Domain.Entities;

namespace CaseApplication.Domain.Dtos
{
    public class UserHistoryOpeningCasesDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }
        public Guid GameCaseId { get; set; }
        public DateTime? CaseOpenAt { get; set; }
    }
}
