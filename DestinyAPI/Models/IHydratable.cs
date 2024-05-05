using DestinyAPI.Models.Interfaces;
using DestinyAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DestinyAPI.Models.Interfaces
{

    public interface IHydratable
    {

    }
    public interface IHydratable<T> : IHydratable
    {

        public IHydratable<T> HydrateFromSource(T value);
    }


    public abstract class Hydratable<T> : IHydratable<T>
    {
        public abstract IHydratable<T> HydrateFromSource(T value);
    }

}

namespace DestinyAPI.Models
{ 

    public static class HydrateExtensions
    {
        public static T1 Hydrate<T1, T2>(this T1 hydratable, T2 source) where T1 : class, IHydratable<T2>
        {
            return (T1)hydratable.HydrateFromSource(source);
        }
    }
}
