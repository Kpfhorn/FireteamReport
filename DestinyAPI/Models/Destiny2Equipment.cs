using DestinyAPI.Models.Interfaces;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models.Destiny;
using DotNetBungieAPI.Models.Destiny.Components;
using DotNetBungieAPI.Models.Destiny.Definitions.InventoryBuckets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Models
{
    public class Destiny2Equipment : IHydratable<DestinyItemComponent>
    {
        public string Name { get; internal set; }
        public DestinyItemType Type { get; internal set; }
        public DestinyItemSubType SubType { get; internal set; }
        public DestinyInventoryBucketDefinition? Bucket {  get; internal set; }
        public string? BucketName => Bucket?.DisplayProperties.Name;



        public virtual IHydratable<DestinyItemComponent> HydrateFromSource(DestinyItemComponent value)
        {
            var item = value.Item.GetValueOrNull();

            Name = item.DisplayProperties.Name;
            Type = item.ItemType;
            SubType = item.ItemSubType;
            Bucket = value.Bucket.GetValueOrNull();
            return this;
        }
    }
}
