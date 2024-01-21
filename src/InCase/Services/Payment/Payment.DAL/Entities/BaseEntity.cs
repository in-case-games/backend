namespace Payment.DAL.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}