using Serilog.Context;

namespace DaprAspire.Gateway.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tenta pegar o header da requisição ou cria um novo
            if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[HeaderName] = correlationId;
            }

            // Garante que a resposta devolva o mesmo header
            context.Response.Headers[HeaderName] = correlationId;

            // Torna acessível via Serilog
            using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
            {
                await _next(context);
            }
        }
    }
}
