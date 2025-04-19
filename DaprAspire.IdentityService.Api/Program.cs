using DaprAspire.IdentityService.Api.Middlewares;
using DaprAspire.IdentityService.Application;

using Microsoft.OpenApi.Models;

using Serilog;

namespace DaprAspire.IdentityService.Api
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

            builder.AddServiceDefaults();

            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddControllers().AddDapr();
            builder.Services.AddDaprClient();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Identity API",
                    Version = "v1",
                    Description = "API para gerenciar identidade de usuários",
                });
            });

            var app = builder.Build();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // /swagger/index.html
            }

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
