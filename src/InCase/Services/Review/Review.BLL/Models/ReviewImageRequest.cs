using Review.DAL.Entities;

namespace Review.BLL.Models;

public class ReviewImageRequest : BaseEntity
{
    public string? Image { get; set; }
    public Guid ReviewId { get; set; }
}