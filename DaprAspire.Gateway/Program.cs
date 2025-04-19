using System.Threading.RateLimiting;

using DaprAspire.Gateway.Identity;
using DaprAspire.Gateway.Middlewares;
using DaprAspire.Gateway.Utilities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Serilog;

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

            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            var logBuilder = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration)
               .Enrich.FromLogContext()
               .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}");

            if (useOtlpExporter)
            {
                logBuilder
               .WriteTo.OpenTelemetry(options =>
               {
                   options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
                   options.ResourceAttributes.Add("service.name", "apiservice");
               });
            }
            ;

            Log.Logger = logBuilder.CreateBootstrapLogger();

            builder.Host.UseSerilog();

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

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("YARP", new OpenApiInfo { Title = "Aggregated Gateway API", Version = "YARP" });

                options.DocumentFilter<SecurityRequirementsDocumentFilter>();
            });

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("AuthenticatedUser", context =>
                {
                    var user = context.User.Identity?.Name ?? "anonymous";
                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: user,
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 10, // máximo de requisições no bucket
                            TokensPerPeriod = 10, // renovação
                            ReplenishmentPeriod = TimeSpan.FromSeconds(30), // intervalo de renovação
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        });
                });
            });

            var app = builder.Build();

            app.UseRateLimiter();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var reverseProxyDocumentFilterConfig = app.Services
                    .GetRequiredService<IOptions<ReverseProxyDocumentFilterConfig>>().Value;

                options.SwaggerEndpoint("/swagger/YARP/swagger.json", "YARP Gateway");
            });

            app.UseRouting();
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");

            app.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.UseRateLimiter();

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
