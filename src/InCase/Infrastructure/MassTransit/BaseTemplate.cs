namespace Infrastructure.MassTransit;

public class BaseTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
}