namespace Infrastructure.MassTransit.Resources;

public class GameItemTemplate : BaseTemplate
{
    public string? Name { get; set; }
    public string? HashName { get; set; }
    public string? IdForMarket { get; set; }
    public decimal Cost { get; set; }
    public bool IsDeleted { get; set; }

    public string? GameName { get; set; }
}