
namespace CareerSphere.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found");
                await WriteErrorAsync(context, 404, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized");
                await WriteErrorAsync(context, 401, "Unauthorized access.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation");
                await WriteErrorAsync(context, 400, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteErrorAsync(context, 500,
                    "Something went wrong. Please try again.");
            }
        }

        private static async Task WriteErrorAsync(
            HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
