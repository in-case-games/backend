using Microsoft.AspNetCore.Mvc;

namespace InCase.Infrastructure.Utils
{
    public class ResponseUtil: ControllerBase
    {
        private const string BAD_REQUEST = "Некорректный запрос";
        private const string SENT_EMAIL = "Сообщение отправлено на email почту";
        private const string PAYMENT_REQUIRED = "Недостаточный баланс";
        private const string FORBIDDEN = "Доступ запрещен";

        public static IActionResult Ok<T>(T data) => CreateResultOK("0", data);

        public static IActionResult Accepted<T>(T data) => CreateResultOK("1", data);

        public static IActionResult SentEmail<T>(T data) => CreateResultOK("2", data);

        public static IActionResult Ok(string message) => CreateResultOK("0", message);

        public new static IActionResult Accepted(string message) => CreateResultOK("1", message);

        public static IActionResult SentEmail(string message = SENT_EMAIL) => 
            CreateResultOK("2", message);

        public static IActionResult BadRequest(string message = BAD_REQUEST) => 
            CreateResultError("0", message);

        public static IActionResult Unauthorized(string message) =>
            CreateResultError("1", message);

        public static IActionResult PaymentRequired(string message = PAYMENT_REQUIRED) =>
            CreateResultError("2", message);

        public static IActionResult Forbidden(string message = FORBIDDEN) =>
            CreateResultError("3", message);

        public static IActionResult NotFound(string message) =>
            CreateResultError("4", message);

        public static IActionResult Conflict(string message) => 
            CreateResultError("5", message);

        public static IActionResult RequestTimeout(string message) =>
            CreateResultError("6", message);

        public static IActionResult UnknownError(string message) => 
            CreateResultError("7", message);

        public static IActionResult UnknownError(Exception ex) =>
            CreateResultError("7", ex.InnerException?.Message ?? ex.Message);


        private static IActionResult CreateResultError(string code, string message) =>
            new BadRequestObjectResult(new
            {
                error = new { code, message }
            });
        private static IActionResult CreateResultOK(string code, string message) =>
            new OkObjectResult(new { code, message });
        private static IActionResult CreateResultOK<T>(string code, T data) => 
            new OkObjectResult(new { code, data });
    }
}
