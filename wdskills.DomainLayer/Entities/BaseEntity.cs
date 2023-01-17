using System.ComponentModel.DataAnnotations;

namespace CaseApplication.DomainLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
