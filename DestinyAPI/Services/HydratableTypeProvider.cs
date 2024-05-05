using DestinyAPI.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Services
{
    public abstract class HydratableTypeProvider<T> where T : notnull, IHydratable
    {
        private IServiceProvider _provider;
        protected ILogger _logger;
        public HydratableTypeProvider(IServiceProvider serviceProvider, ILogger logger)
        {
            _provider = serviceProvider;
            _logger = logger;
        }

        public T GetInstance()
        {
            try
            {
               return _provider.GetRequiredService<T>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when trying to instantiate an object of type {hydrateType}.", typeof(T).Name);
                throw;
            }


        }


        public T2 GetInstance<T2>() where T2: T
        {
            try
            {
                return _provider.GetRequiredService<T2>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when trying to instantiate an object of type {hydrateType}.", typeof(T).Name);
                throw;
            }
        }
    }
}
