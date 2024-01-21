using Promocode.BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace Promocode.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
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

    private static Task HandleExceptionAsync(HttpContext context, StatusCodeException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(new
        {
            error = new { code = ex.StatusCode, message = ex.Message }
        });

        return context.Response.WriteAsync(result);
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