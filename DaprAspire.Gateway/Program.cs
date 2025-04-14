using DaprAspire.Gateway.Utilities;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

namespace DaprAspire.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Carrega a seção ReverseProxy do appsettings.json
            var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");

            // Adiciona o YARP e o Swagger
            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(reverseProxyConfig)
                .AddSwagger(reverseProxyConfig);

            // Configura o SwaggerGen com um documento padrão
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("YARP", new OpenApiInfo { Title = "Aggregated Gateway API", Version = "YARP" });
            });

            // Adiciona a configuração personalizada do Swagger
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            var app = builder.Build();

            // Ativa o Swagger e o Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Obtém a configuração do Swagger para os clusters
                var reverseProxyDocumentFilterConfig = app.Services.GetRequiredService<IOptions<ReverseProxyDocumentFilterConfig>>().Value;

                // Adiciona o endpoint agregado ao Swagger UI
                options.SwaggerEndpoint("/swagger/YARP/swagger.json", "YARP Gateway");
            });

            app.MapReverseProxy();

            app.Run();
        }
    }
}
