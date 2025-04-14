using DaprAspire.Consolidation.Application;

using Microsoft.OpenApi.Models;

namespace DaprAspire.ConsolidationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Add services to the container.

            builder.Services.AddControllers().AddDapr();
            builder.Services.AddDaprClient();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddMongo(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddProjectionServices();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Consolidation API",
                    Version = "v1",
                    Description = "API para consolidar e consultar lançamentos financeiros",
                });
            });

            var app = builder.Build();

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
            app.UseCloudEvents();
            app.MapSubscribeHandler();

            app.Run();
        }
    }
}
