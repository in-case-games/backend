namespace wdskills.DomainLayer.Entities
{
    public class UserEffects : BaseEntity
    {
        public User? User { get; set; }
        public string? EffectName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}