using Microsoft.AspNetCore.Mvc;

namespace InCase.Infrastructure.Utils
{
    public class ResponseUtil: ControllerBase
    {
        /*
         STATUS CODE ERROR(400):
            0 - Bad Request
            1 - Unauthorized
            2 - Payment Required (Недостаточный баланс)
            3 - Forbidden (Доступ запрещен)
            4 - Not Found (Не найдено запись в бд)
            5 - Conflict (Конфликт возможно запись уже есть)
            6 - Unknown Error (Ошибку поймал try/catch)
         STATUS CODE OK(200):
            0 - OK
            1 - Accepted (Принято в обработку)
            2 - Sent Email (Отправлено на почту)
        */

        public static IActionResult Ok<T>(T data) => new OkObjectResult(new { code = "0", data });

        public static IActionResult Accepted<T>(T data) => new OkObjectResult(new { code = "1", data });

        public static IActionResult SentEmail<T>(T data) => new OkObjectResult(new { code = "2", data });

        public static IActionResult Ok(string msg) => new OkObjectResult(new { code = "0", msg });

        public new static IActionResult Accepted(string msg) => new OkObjectResult(new { code = "1", msg });

        public static IActionResult SentEmail(string msg = "Сообщение отправлено на email почту") => 
            new OkObjectResult(new { code = "2", msg });

        public static IActionResult BadRequest(string msg = "Некорректный запрос") => 
            new BadRequestObjectResult(new
            {
                error = new { code = "0", msg }
            });

        public static IActionResult Unauthorized(string msg) =>
            new BadRequestObjectResult(new
            {
                error = new { code = "1", msg }
            });

        public static IActionResult PaymentRequired(string msg = "Недостаточный баланс") => 
            new BadRequestObjectResult(new
            {
                error = new { code = "2", msg }
            });

        public static IActionResult Forbidden(string msg = "Доступ запрещен") => 
            new BadRequestObjectResult(new
            {
                error = new { code = "3", msg }
            });

        public static IActionResult NotFound(string msg) => 
            new BadRequestObjectResult(new
            {
                error = new { code = "4", msg }
            });

        public static IActionResult Conflict(string msg) => 
            new BadRequestObjectResult(new
            {
                error = new { code = "5", msg }
            });

        public static IActionResult UnknownError(string msg) => 
            new BadRequestObjectResult(new
            {
                error = new { code = "6", msg }
            });

        public static IActionResult UnknownError(Exception ex) => 
            new BadRequestObjectResult(new
            {
                error = new { code = "6", msg = ex.InnerException?.Message ?? ex.Message }
            });
    }
}
