using DestinyAPI.Models;
using DestinyAPI.Services;
using DotNetBungieAPI;
using DotNetBungieAPI.Service.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.DependencyInjection
{
    public class DestinyAPIBuilder
    {
        private IServiceCollection _serviceCollection;
        private Action<IBungieClientBuilder>? _configureClient;

        public bool FetchLatestManifestOnInitialize { get; set; } = false;

        internal DestinyAPIBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        internal void ValidateAndRegisterServices()
        {
            if (_configureClient is null) throw new Exception("Bungie client has not been configured!");
                _serviceCollection.UseBungieApiClient(_configureClient);


            AddProviders("DestinyAPI.Services");
            AddModels("DestinyAPI.Models");
            //_serviceCollection.AddSingleton<BungieClientProvider>();
            //_serviceCollection.AddSingleton<DestinyUserProvider>();
            //_serviceCollection.AddSingleton<DestinyCharacterProvider>();
            //_serviceCollection.AddSingleton<DestinyEquipmentProvider>();
            //_serviceCollection.AddTransient<BungieUser>();
            //_serviceCollection.AddTransient<Destiny2Character>();
            //_serviceCollection.AddTransient<Destiny2Equipment>();
            //_serviceCollection.AddTransient<Destiny2Weapon>();
            _serviceCollection.AddTransient<Destiny2>();

            if(FetchLatestManifestOnInitialize)
            {
                _serviceCollection.AddHostedService<FetchManifestService>();
            }
            
        }

        internal void AddModels(string modelNamespace)
        {
           var assembly = Assembly.GetAssembly(typeof(DestinyAPIBuilder));

            if (assembly is null) return;

            foreach(var type in assembly.DefinedTypes)
            {
                if(!type.IsAbstract
                    && !type.IsInterface
                    && type.IsPublic
                    && type.Namespace.Equals(modelNamespace))
                {
                    _serviceCollection.AddTransient(type.AsType());
                }
            }
        }

        internal void AddProviders(string providerNamespace)
        {
            var assembly = Assembly.GetAssembly(typeof(DestinyAPIBuilder));

            if (assembly is null) return;

            foreach (var type in assembly.DefinedTypes)
            {
                if (!type.IsAbstract
                    && !type.IsInterface
                    && type.IsPublic
                    && type.Namespace.Equals(providerNamespace))
                {
                    _serviceCollection.AddSingleton(type.AsType());
                }
            }
        }
     
        public DestinyAPIBuilder ConfigureBungieApiClient(Action<IBungieClientBuilder> configure)
        {
            _configureClient = configure;
            return this;
        }
    }
}
