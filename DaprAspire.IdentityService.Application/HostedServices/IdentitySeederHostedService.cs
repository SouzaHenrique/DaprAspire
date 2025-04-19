
using DaprAspire.IdentityService.Application.Services.Seeders;

using DnsClient.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DaprAspire.IdentityService.Application.HostedServices
{
    /// <summary>
    /// Executes the IdentitySeeder at application startup.
    /// </summary>
    public class IdentitySeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IdentitySeederHostedService> _logger;
        public IdentitySeederHostedService(IServiceProvider serviceProvider, ILogger<IdentitySeederHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.CompletedTask;

            using var scope = _serviceProvider.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();

            await seeder.SeedAsync(cancellationToken);

            _logger.LogInformation("IdentitySeeder executed via hosted service.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
