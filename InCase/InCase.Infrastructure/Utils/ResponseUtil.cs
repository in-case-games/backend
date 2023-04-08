using Microsoft.AspNetCore.Mvc;

namespace InCase.Infrastructure.Utils
{
    public class ResponseUtil: ControllerBase
    {
        public static IActionResult Ok<T>(T entity)
        {
            return new OkObjectResult(new { Success = true, Data = entity });
        }
        public static IActionResult NotFound(string name, string description = "")
        {
            return new NotFoundObjectResult(new { 
                Success = false,
                Data = $"{name} is not found. {description}" });
        }
        public static IActionResult Conflict(string message)
        {
            return new ConflictObjectResult(new
            {
                Success = false,
                Data = message
            });
        }
        public static IActionResult Error(Exception ex)
        {
            return new ConflictObjectResult(new {
                Success = false,
                Data = ex.InnerException?.Message });
        }
        public static IActionResult Accept(string message = "")
        {
            return new AcceptedResult(location: null, new
            {
                Success = true,
                Data = message
            });
        }
        public static IActionResult SendEmail()
        {
            return Accept("Message was sended on your email");
        }
    }
}
