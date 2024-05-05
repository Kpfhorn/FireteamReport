using DotNetBungieAPI.Models.Destiny.Components;
using DotNetBungieAPI.Models;
using DotNetBungieAPI.Models.Destiny.Definitions.Stats;
using DotNetBungieAPI.Models.Destiny;
using DotNetBungieAPI.Models.Extensions;
using System.Runtime.InteropServices;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Service.Abstractions;
using DotNetBungieAPI.Models.Destiny.Definitions.Classes;
using Microsoft.Data.Sqlite;
using DestinyAPI.Services;
using DestinyAPI.Extensions;
using DestinyAPI.Models.Interfaces;

namespace DestinyAPI.Models
{
    public class Destiny2Character : Hydratable<DestinyCharacterComponent>
    {
        private DestinyEquipmentProvider equipmentProvider;
        private IAsyncEnumerable<Destiny2Equipment> equippedItems;

        public Destiny2Character(DestinyEquipmentProvider equiment) 
        {
            equipmentProvider = equiment;
        }

        public string? Gender { get; internal set; }
        public string? EmblemBackground { get; internal set; }
        public string? Emblem { get; internal set; }
        public string? Class { get; internal set; }
        public string? Race { get; internal set; }
        public string? Title { get; internal set; }
        public int PowerLevel { get; internal set; }
        public int Mobility { get; internal set; }
        public int Resilience { get; internal set; }
        public int Recovery { get; internal set; }
        public int Discipline { get; internal set; }
        public int Intellect { get; internal set; }
        public int Strength { get; internal set; }
        public DateTime? DateLastPlayed { get; internal set; }

        public long MembershipId { get; internal set; }
        public long CharacterId { get; internal set; }
        public BungieMembershipType MembershipType { get; internal set; }

        public IAsyncEnumerable<Destiny2Equipment> Equipped
        {
            get
            {
                equippedItems ??= equipmentProvider.GetEquipment(this);
                return equippedItems;
            }
        }
        public IAsyncEnumerable<Destiny2Weapon> EquippedWeapons => Equipped.OfType<Destiny2Weapon>();
        public IAsyncEnumerable<Destiny2Armor> EquippedArmor => Equipped.OfType<Destiny2Armor>();
        public ValueTask<Destiny2Subclass?> EquippedSubclass => Equipped.OfType<Destiny2Subclass>().FirstOrDefaultAsync();

        public override IHydratable<DestinyCharacterComponent> HydrateFromSource(DestinyCharacterComponent value)
        {
            Gender = value.Gender.GetValueOrNull()?.DisplayProperties.Name;
            EmblemBackground = value.EmblemBackgroundPath;
            Emblem = value.EmblemPath;
            Class = value.Class.GetValueOrNull()?.DisplayProperties.Name;
            PowerLevel = value.Light;
            DateLastPlayed = value.DateLastPlayed;
            Race = value.Race.GetValueOrNull()?.DisplayProperties.Name;
            Title = value.TitleRecord.GetValueOrNull()?.DisplayProperties.Name;

            Mobility = value.Stats.GetStatValue(nameof(Mobility));
            Resilience = value.Stats.GetStatValue(nameof(Resilience));
            Recovery = value.Stats.GetStatValue(nameof(Recovery));
            Discipline = value.Stats.GetStatValue(nameof(Discipline));
            Intellect = value.Stats.GetStatValue(nameof(Intellect));
            Strength = value.Stats.GetStatValue(nameof(Strength));
            MembershipId = value.MembershipId;
            MembershipType = value.MembershipType;
            CharacterId = value.CharacterId;
            return this;
        }
    }
}
