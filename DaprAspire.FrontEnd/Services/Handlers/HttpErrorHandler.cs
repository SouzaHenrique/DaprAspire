namespace DaprAspire.FrontEnd.Http
{
    public class HttpErrorHandler : DelegatingHandler
    {
        private readonly ILogger<HttpErrorHandler> _logger;

        public HttpErrorHandler(ILogger<HttpErrorHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null!;
            try
            {
                response = await base.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("API error: {StatusCode} - {Body}", response.StatusCode, body);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP request failed: {Method} {Url}", request.Method, request.RequestUri);
                throw;
            }
        }
    }
}
