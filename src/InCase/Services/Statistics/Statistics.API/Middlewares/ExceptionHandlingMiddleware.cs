using System.Text.Json;

namespace Statistics.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                await HandleExceptionAsync(context, "Task was cancelled");

                _logger.LogWarning("Task was cancelled");
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
