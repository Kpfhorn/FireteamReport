using DestinyAPI.Extensions;
using DestinyAPI.Models;
using DestinyAPI.Services;
using DotNetBungieAPI.Service.Abstractions;

namespace DestinyAPI
{
    public class Destiny2
    {

        private DestinyUserProvider _userProvider;
        private BungieClientProvider _clientProvider;

        public Destiny2(BungieClientProvider clientProvider, DestinyUserProvider userProvider)
        {
            _userProvider = userProvider;
            _clientProvider = clientProvider;
        }

        public IAsyncEnumerable<BungieUser> SearchUserAsync(string searchText)
        {
            return _userProvider.SearchUserAsync(searchText);
        }


    }
}
