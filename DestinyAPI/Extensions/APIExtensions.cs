using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models;
using DotNetBungieAPI.Models.Destiny.Definitions.Stats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Extensions
{
    public static class APIExtensions
    {
        internal static int GetStatValue(this ReadOnlyDictionary<DefinitionHashPointer<DestinyStatDefinition>, int> stats, string statName)
        {
            var keys = stats.Keys;
            return stats.Where(stat => stat
                                        .Key
                                        .GetValueOrNull()
                                        ?.DisplayProperties
                                        ?.Name
                                        ?.Equals(statName)
                                        ?? false)
                                        .FirstOrDefault()
                                        .Value;
        }

    }
}
