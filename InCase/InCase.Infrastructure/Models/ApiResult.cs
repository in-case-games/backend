using Microsoft.AspNetCore.Mvc;

namespace InCase.Infrastructure.Models
{
    public class ApiResult : ControllerBase
    {
        private const string SENT_EMAIL = "Сообщение отправлено на email почту";

        public static IActionResult OK<T>(T data) => 
            CreateResultOK(0, data);
        public static IActionResult Accepted<T>(T data) => 
            CreateResultOK(1, data);
        public static IActionResult SentEmail(string message = SENT_EMAIL) => 
            CreateResultOK(2, message);

        private static IActionResult CreateResultOK<T>(int code, T data) =>
            new OkObjectResult(new { code, data });
    }
}
