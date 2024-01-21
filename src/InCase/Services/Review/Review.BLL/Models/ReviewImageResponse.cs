using Review.DAL.Entities;

namespace Review.BLL.Models;

public class ReviewImageResponse : BaseEntity
{
    public Guid ReviewId { get; set; }
}