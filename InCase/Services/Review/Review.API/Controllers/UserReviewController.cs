using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review.BLL.Interfaces;

namespace Review.API.Controllers
{
    [Route("api/user-review")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly IUserReviewService _userReviewService;

        public UserReviewController(IUserReviewService userReviewService)
        {
            _userReviewService = userReviewService;
        }
    }
}
