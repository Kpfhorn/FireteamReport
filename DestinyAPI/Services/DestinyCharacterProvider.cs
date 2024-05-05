using DestinyAPI.Models;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models.Destiny;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Services
{
    public class DestinyCharacterProvider(
        IServiceProvider serviceProvider, 
        BungieClientProvider clientProvider,
        ILogger<DestinyCharacterProvider> logger) : HydratableTypeProvider<Destiny2Character>(serviceProvider, logger)
    {
        public async IAsyncEnumerable<Destiny2Character?> GetCharactersAsync(BungieUser user, [EnumeratorCancellation]CancellationToken cancellationToken)
        {

            foreach (var membership in user.DestinyMemberships)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var response = await clientProvider.Client.ApiAccess.Destiny2.GetProfile
                    (
                    membership.MembershipType,
                    membership.MembershipId,
                    [
                        DestinyComponentType.Characters
                    ]
                );

                if (!response.IsSuccessfulResponseCode)
                    continue;

                foreach(var character in response.Response.Characters.Data.Values)
                {
                    var instance = GetInstance().Hydrate(character);
                    _logger.LogDebug(CharacterLogTemplate, 
                        instance.Class,
                        instance.PowerLevel,
                        instance.Race,
                        instance.Title);
                    yield return GetInstance().Hydrate(character);
                }
            }


        }

        private const string CharacterLogTemplate =
            @"Retrieved Character from API:
            Class: {class}
            Power: {power}
            Race: {race}
            Title: {title}";


    }
}
