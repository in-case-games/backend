using InCase.Infrastructure.CustomException;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace InCase.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (StatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, StatusCodeException ex)
        {
            string result;
            context.Response.ContentType = "application/json";

            int code = ex.StatusCode;
            string message = ex.Message;

            result = JsonSerializer.Serialize(new
            {
                error = new { code, message }
            });
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result;
            context.Response.ContentType = "application/json";

            int code = (int)HttpStatusCode.InternalServerError;
            string message = ex.Message;

            result = JsonSerializer.Serialize(new
            {
                error = new { code, message }
            });
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(result);
        }
    }
}
