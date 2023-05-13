using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InCase.Infrastructure.Middlewares
{
    public class CancellationTokenHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CancellationTokenHandlingMiddleware> _logger;

        public CancellationTokenHandlingMiddleware(
            RequestDelegate next, 
            ILogger<CancellationTokenHandlingMiddleware> logger)
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
            catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
            {
                _logger.LogWarning("Task was cancelled");
            }
        }
    }
}
