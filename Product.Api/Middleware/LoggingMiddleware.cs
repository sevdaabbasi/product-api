using System.Diagnostics;

namespace Product.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation($"Request started: {context.Request.Method} {context.Request.Path}");
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"Request completed: {context.Request.Method} {context.Request.Path} in {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
} 