using Resources.BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace Resources.API.Middlewares
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
            catch (StatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
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

        private static Task HandleExceptionAsync(HttpContext context, StatusCodeException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            string result = JsonSerializer.Serialize(new
            {
                error = new { code = ex.StatusCode, message = ex.Message }
            });

            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, string message)
        {
            int internalServerErrorCode = 500;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = internalServerErrorCode;

            string result = JsonSerializer.Serialize(new
            {
                error = new { code = internalServerErrorCode, message = message }
            });

            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int internalServerErrorCode = 500;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = internalServerErrorCode;

            string result = JsonSerializer.Serialize(new
            {
                error = new { code = internalServerErrorCode, message = ex.Message }
            });

            return context.Response.WriteAsync(result);
        }
    }
}
