using DotNetBungieAPI.Models.Destiny.Components;
using DotNetBungieAPI.Models.Destiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetBungieAPI.Extensions;
using DestinyAPI.Models.Interfaces;

namespace DestinyAPI.Models
{
    public class Destiny2Subclass : Destiny2Equipment
    {

        public override IHydratable<DestinyItemComponent> HydrateFromSource(DestinyItemComponent value)
        {
            base.HydrateFromSource(value);
            var item = value.Item.GetValueOrNull();


            return this;
        }
    }
}
