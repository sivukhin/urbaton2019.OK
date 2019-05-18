using System;
using System.Linq;
using System.Threading;

namespace CleanCityCore
{
    public static class CollectionExtensions
    {
        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(() => new Random());
        public static T[] Shuffle<T>(this T[] data)
        {
            var copy = data.ToArray();
            for (var i = 1; i < data.Length; i++)
            {
                var j = Random.Value.Next(i);
                var value = copy[j];
                copy[j] = copy[i];
                copy[i] = value;
            }

            return copy;
        }
    }
}