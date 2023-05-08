using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InCase.Infrastructure.Middleware
{
    public class TaskCancellationTokenHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TaskCancellationTokenHandleMiddleware> _logger;

        public TaskCancellationTokenHandleMiddleware(RequestDelegate next, ILogger<TaskCancellationTokenHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
            {
                _logger.LogWarning("Task was cancelled");
            }
        }
    }
}
