using DestinyAPI.Models;
using DotNetBungieAPI.Extensions;
using DotNetBungieAPI.Models.Destiny;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Services
{
    public class DestinyEquipmentProvider(
        IServiceProvider serviceProvider,
        BungieClientProvider clientProvider,
        ILogger<DestinyEquipmentProvider> logger) : HydratableTypeProvider<Destiny2Equipment>(serviceProvider, logger)
    {

        public async IAsyncEnumerable<Destiny2Equipment> GetEquipment(Destiny2Character character)
        {
            var response = await clientProvider.Client.ApiAccess.Destiny2.GetCharacter
                (
                    character.MembershipType,
                    character.MembershipId,
                    character.CharacterId,
                    [
                        DestinyComponentType.CharacterEquipment
                    ]
                );

            foreach(var item in response.Response.Equipment.Data.Items)
            {
                switch (item.Item.GetValueOrNull().ItemType)
                {
                    case DestinyItemType.Weapon:
                        var weapon = GetInstance<Destiny2Weapon>().Hydrate(item);
                        _logger.LogDebug(WeaponLogTemplate,
                            weapon.Name,
                            weapon.Type.ToString(),
                            weapon.ItemSubType.ToString(),
                            weapon.DamageType,
                            weapon.BucketName);
                        yield return weapon;
                        break;
                    case DestinyItemType.Armor:
                        var armor = GetInstance<Destiny2Armor>().Hydrate(item);
                        _logger.LogDebug(EquipmentLogTemplate,
                            armor.Name,
                            armor.Type,
                            armor.SubType,
                            armor.BucketName);
                        yield return armor;
                        break;
                    case DestinyItemType.Subclass:
                        var subclass = GetInstance<Destiny2Subclass>().Hydrate(item);
                        _logger.LogDebug(SubclassLogTemplate,
                            subclass.Name,
                            subclass.Type,
                            subclass.BucketName);
                        yield return subclass;
                        break;
                    default:
                        var other = GetInstance().Hydrate(item);
                        _logger.LogDebug(EquipmentLogTemplate,
                            other.Name,
                            other.Type, 
                            other.SubType,
                            other.BucketName);
                        yield return other;
                        break;
                }
            }
        }


        private const string EquipmentLogTemplate =
            @"Retrieved Item from API:
            Name: {name}
            Type: {type}
            SubType: {subtype}
            Bucket: {bucket}";

        private const string WeaponLogTemplate =
            @"Retrieved Weapon from API:
            Name: {name}
            Type: {type}
            SubType: {subType}
            DamageType: {damageType}
            Bucket: {bucket}";

        private const string SubclassLogTemplate =
            @"Retrieved Subclass from API:
            Name: {name}
            Type: {type}
            Bucket: {bucket}";

    }
}
