using Microsoft.AspNetCore.Mvc;

namespace SupportTopic.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupportTopicController : Controller
    {
        private readonly ILogger<SupportTopicController> _logger;

        public SupportTopicController(ILogger<SupportTopicController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}