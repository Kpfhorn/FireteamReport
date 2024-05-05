using DestinyAPI.Models.Interfaces;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models.Destiny;
using DotNetBungieAPI.Models.Destiny.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Models
{
    public class Destiny2Weapon : Destiny2Equipment
    {
        public DamageType DamageType { get; internal set; }
        public DestinyItemSubType ItemSubType { get; internal set; }
        public int Power { get; internal set; }
        public override IHydratable<DestinyItemComponent> HydrateFromSource(DestinyItemComponent value)
        {
            base.HydrateFromSource(value);
            var item = value.Item.GetValueOrNull();

            DamageType = item.DefaultDamageType.GetValueOrNull().EnumValue;
            ItemSubType = item.ItemSubType;


            return this;
        }
    }
}
