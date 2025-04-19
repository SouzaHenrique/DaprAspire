using Microsoft.AspNetCore.Http;

using Serilog.Context;

using System;
using System.Threading.Tasks;

namespace DaprAspire.ConsolidationApi.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId;

            if (context.Request.Headers.ContainsKey(CorrelationIdHeader))
            {
                correlationId = context.Request.Headers[CorrelationIdHeader]!;
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationIdHeader] = correlationId;
            }

            context.Items[CorrelationIdHeader] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
