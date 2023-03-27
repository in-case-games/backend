using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
