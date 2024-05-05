using DestinyAPI.Extensions;
using DestinyAPI.Models;
using DotNetBungieAPI.Models.User;
using DotNetBungieAPI.Service.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Services
{
    public class DestinyUserProvider(
        IServiceProvider services, 
        BungieClientProvider provider, 
        ILogger<DestinyUserProvider> logger) : HydratableTypeProvider<BungieUser>(services, logger)
    {
        public async IAsyncEnumerable<BungieUser> SearchUserAsync(string searchText) 
        {
            await foreach(var user in provider.Client.SearchUsersAsync(searchText))
            {
                _logger.LogDebug("Retrieved user {displayName}#{id} from Bungie API", user.BungieGlobalDisplayName, user.BungieGlobalDisplayNameCode);
                yield return GetInstance().Hydrate(user);
            }
            
        }
    }
}
