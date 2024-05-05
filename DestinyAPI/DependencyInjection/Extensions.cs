using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.DependencyInjection
{
    public static class Extensions
    {
        public static IServiceCollection UseDestinyAPI(this IServiceCollection services, Action<DestinyAPIBuilder> configure)
        {
            var builder = new DestinyAPIBuilder(services);
            configure(builder);

            builder.ValidateAndRegisterServices();

            return services;
        }
    }
}
