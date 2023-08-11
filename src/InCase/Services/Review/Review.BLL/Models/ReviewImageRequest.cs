using Microsoft.Extensions.FileProviders;
using Review.DAL.Entities;

namespace Review.BLL.Models
{
    public class ReviewImageRequest : BaseEntity
    {
        public Guid ReviewId { get; set; }
    }
}
