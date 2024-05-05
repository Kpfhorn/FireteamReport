

using DotNetBungieAPI.Service.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace DestinyAPI.Services
{
    public class FetchManifestService : IHostedLifecycleService
    {
        private readonly BungieClientProvider _clientProvider;
        private readonly ILogger<FetchManifestService> _logger;

        public FetchManifestService(
            BungieClientProvider clientProvider,
             ILogger<FetchManifestService> logger)
        {
            _clientProvider = clientProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StartingAsync(CancellationToken cancellationToken)
        {
            await UpdateManifest();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task UpdateManifest()
        {
            try
            {
                _logger.LogInformation("Updating manifest...");
                await _clientProvider.Initialize();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch manifest.");
                return;
            }
        }
    }
}