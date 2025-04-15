using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

using Yarp.ReverseProxy.Swagger;

namespace DaprAspire.Gateway.Utilities
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            // Apenas aplica o filtro de agregação
            options.DocumentFilter<ReverseProxyDocumentFilter>();
        }
    }
}
