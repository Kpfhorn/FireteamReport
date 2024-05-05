using DestinyAPI.Models;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models;
using DotNetBungieAPI.Models.Destiny;
using DotNetBungieAPI.Models.Requests;
using DotNetBungieAPI.Models.User;
using DotNetBungieAPI.Service.Abstractions;

namespace DestinyAPI.Extensions;

public static class UserExtensions
{
    public static IEnumerable<UserSearchResponseDetail> AsUserEnumerable(this UserSearchResponse response)
    {
        foreach (var userDetail in response.SearchResults)
        {
            yield return userDetail;
        }
    }

    public static async IAsyncEnumerable<UserSearchResponseDetail> SearchUsersAsync(
        this IBungieClient client,
        string searchText,
        int page = 0,
        bool allPages = false)
    {
        BungieResponse<UserSearchResponse> response;
        
        response = await client.ApiAccess.User.SearchByGlobalNamePost(
                new UserSearchPrefixRequest(searchText),
                page);
        
        if (!response.IsSuccessfulResponseCode)
            throw response.ToException();

        foreach (var detail in response.Response.SearchResults)
        {
            yield return detail;
        }

        if (response.Response.HasMore && allPages)
        {
            await foreach (var detail in client.SearchUsersAsync(searchText, ++page, allPages))
            {
                yield return detail;
            }
        }
    }


    public static async Task<BungieUser> GetBungieUserAsync(this BungieUser user)
    {
        return user;
    }

    public static async Task<BungieUser> ResolveMembership(this Task<BungieUser> userTask)
    {
        var user = await userTask;

        UserInfoCard? primaryMembership;

        if (user.DestinyMemberships.Count > 1)
            primaryMembership = user.DestinyMemberships.Aggregate((info1, info2) =>
            {
                if(info2.MembershipType == info2.СrossSaveOverride)
                {
                    return info2;
                }
                else
                {
                    return info1;
                }
            });
        else
            primaryMembership = user.DestinyMemberships.FirstOrDefault();

        user.PrimaryMembership = primaryMembership;

        return user;
    }
}