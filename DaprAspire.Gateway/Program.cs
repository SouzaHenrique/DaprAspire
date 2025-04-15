using DaprAspire.Gateway.Identity;
using DaprAspire.Gateway.Utilities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

#pragma warning disable ASP0013
            builder.Host.ConfigureAppConfiguration((context, config) =>
            {
                config.AddUserSecrets<Program>();
            });
#pragma warning restore ASP0013

            var configuration = builder.Configuration;
            var reverseProxyConfig = configuration.GetSection("ReverseProxy");

            // Configura autenticação JWT via método utilitário
            builder.Services.AddAuth(configuration);
            builder.Services.AddAuthorization();

            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(reverseProxyConfig)
                .AddSwagger(reverseProxyConfig);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("YARP", new OpenApiInfo { Title = "Aggregated Gateway API", Version = "YARP" });

                options.DocumentFilter<SecurityRequirementsDocumentFilter>();
            });

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var reverseProxyDocumentFilterConfig = app.Services
                    .GetRequiredService<IOptions<ReverseProxyDocumentFilterConfig>>().Value;

                options.SwaggerEndpoint("/swagger/YARP/swagger.json", "YARP Gateway");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.Use(async (context, next) =>
                {
                    var path = context.Request.Path.Value;

                    var isPublic =
                        path?.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) == true ||
                        path?.Contains("/swagger.json", StringComparison.OrdinalIgnoreCase) == true ||
                        path?.Equals("/identity/api/Account/login", StringComparison.OrdinalIgnoreCase) == true;

                    if (isPublic)
                    {
                        await next();
                        return;
                    }

                    // Autentica normalmente
                    var authService = context.RequestServices.GetRequiredService<IAuthenticationService>();
                    var authResult = await authService.AuthenticateAsync(context, JwtBearerDefaults.AuthenticationScheme);

                    if (!authResult.Succeeded)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    await next();
                });
            });

            // Pré-carrega o Swagger unificado
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(() =>
                {
                    using var scope = app.Services.CreateScope();
                    var swagger = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();

                    try
                    {
                        swagger.GetSwagger("YARP");
                        Console.WriteLine("Swagger YARP pré-carregado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Falha ao gerar Swagger YARP no startup: " + ex.Message);
                    }
                });
            });

            app.Run();
        }
    }
}
