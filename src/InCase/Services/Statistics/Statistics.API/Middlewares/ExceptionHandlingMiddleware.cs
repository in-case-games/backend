using System.Text.Json;

namespace Statistics.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException)
            {
                await HandleExceptionAsync(context, "Task was cancelled");

                logger.LogWarning("Task was cancelled");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string message)
        {
            const int internalServerErrorCode = 500;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = internalServerErrorCode;

            var result = JsonSerializer.Serialize(new
            {
                error = new { code = internalServerErrorCode, message }
            });

            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            const int internalServerErrorCode = 500;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = internalServerErrorCode;

            var result = JsonSerializer.Serialize(new
            {
                error = new { code = internalServerErrorCode, message = ex.Message }
            });

            return context.Response.WriteAsync(result);
        }
    }
}
