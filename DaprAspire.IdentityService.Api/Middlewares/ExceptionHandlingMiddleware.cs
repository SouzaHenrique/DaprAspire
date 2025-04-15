namespace DaprAspire.IdentityService.Api.Middlewares
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
                await _next(context); // Executa o pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    Title = "An unexpected error occurred.",
                    Status = 500,
#if DEBUG
                    Detail = ex.Message
#else
                    Detail = "Internal error"
#endif
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

}
