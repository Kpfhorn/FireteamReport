using System.Collections.ObjectModel;
using DestinyAPI.Services;
using DotNetBungieAPI.Models.Common;
using DotNetBungieAPI.Models.Destiny.Components;
using DotNetBungieAPI.Models.User;
using System.Linq;
using DestinyAPI.Models.Interfaces;

namespace DestinyAPI.Models;

public class BungieUser : Hydratable<UserSearchResponseDetail>
{

    private DestinyCharacterProvider _characterProvider;
    
    public BungieUser(DestinyCharacterProvider characterProvider)
    {
        _characterProvider = characterProvider;
    }

    public string DisplayName => $"{BungieGlobalDisplayName}#{BungieGlobalDisplayNameCode}";

    public string? BungieGlobalDisplayName { get; internal set; }

    public short? BungieGlobalDisplayNameCode { get; internal set; }

    public long BungieNetMembershipId { get; internal set; }

    public ReadOnlyCollection<UserInfoCard>? DestinyMemberships { get; internal set; }

    public UserInfoCard PrimaryMembership { get; set; } = new();

    public IAsyncEnumerable<Destiny2Character?> Characters => _characterProvider.GetCharactersAsync(this, CancellationToken.None);
    public ValueTask<Destiny2Character?> LastPlayedCharacter => Characters.OrderByDescending(character => character.DateLastPlayed).FirstOrDefaultAsync();


    public override IHydratable<UserSearchResponseDetail> HydrateFromSource(UserSearchResponseDetail value)
    {
        BungieGlobalDisplayName = value.BungieGlobalDisplayName;
        BungieGlobalDisplayNameCode = value.BungieGlobalDisplayNameCode;
        BungieNetMembershipId = value.BungieNetMembershipId;
        DestinyMemberships = value.DestinyMemberships;
        return this;
    }

    public override string ToString()
    {
        return DisplayName;
    }

}