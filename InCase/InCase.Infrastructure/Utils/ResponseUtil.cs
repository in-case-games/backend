using Microsoft.AspNetCore.Mvc;

namespace InCase.Infrastructure.Utils
{
    public class ResponseUtil: ControllerBase
    {
        private const string SENT_EMAIL = "Сообщение отправлено на email почту";

        public static IActionResult Ok<T>(T data) => CreateResultOK("0", data);

        public static IActionResult Accepted<T>(T data) => CreateResultOK("1", data);

        public static IActionResult SentEmail(string message = SENT_EMAIL) => 
            CreateResultOK("2", message);

        private static IActionResult CreateResultOK<T>(string code, T data) => 
            new OkObjectResult(new { code, data });
    }
}
