using DotNetBungieAPI.Service.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Services
{
    public class BungieClientProvider
    {
        private ILogger _logger;
        private IBungieClient _bungieClient;
        private bool _initialized = false;

        public BungieClientProvider(IBungieClient bungieClient, ILogger<BungieClientProvider> logger)
        {
            _bungieClient = bungieClient;
            _logger = logger;
        }


        public async Task Initialize()
        {
            try
            {
                if (!_initialized)
                {
                    _logger.LogDebug("Initializing Bungie API client...");
                    await _bungieClient.DefinitionProvider.Initialize();
                    _initialized = true;
                    _logger.LogDebug("Bungie API client initialized.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing the internal Bungie API client.");
                throw;
            }
        }

        public IBungieClient Client
        {
            get
            {
                try
                {
                    if (!_initialized)
                    {
                        _logger.LogDebug("Initializing Bungie API client...");
                        var task = _bungieClient.DefinitionProvider.Initialize();
                        task.Wait();
                        _initialized = true;
                        _logger.LogDebug("Bungie API client initialized.");
                    }
                    return _bungieClient;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while initializing the internal Bungie API client.", []);
                    throw;
                }
            }
        }
    }
}
