
using DaprAspire.IdentityService.Application.Services.Seeders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaprAspire.IdentityService.Application.HostedServices
{
    /// <summary>
    /// Executes the IdentitySeeder at application startup.
    /// </summary>
    public class IdentitySeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public IdentitySeederHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.CompletedTask;

            using var scope = _serviceProvider.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();

            await seeder.SeedAsync(cancellationToken);

            Console.WriteLine("IdentitySeeder executed via hosted service.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
