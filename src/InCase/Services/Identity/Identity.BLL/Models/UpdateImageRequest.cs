namespace Identity.BLL.Models;
public class UpdateImageRequest
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string? Image { get; set; }
}