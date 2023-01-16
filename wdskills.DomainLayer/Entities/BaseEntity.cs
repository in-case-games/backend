using System.ComponentModel.DataAnnotations;

namespace wdskills.DomainLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
