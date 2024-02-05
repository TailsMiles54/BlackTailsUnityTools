using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackTailsUnityTools.Scripts.Utils
{
    public static class CollectionUtils
    {
        public static TSource GetRandomElement<TSource>(this IEnumerable<TSource> collection)
        {
            var idx = Random.Range(0, collection.Count());
            return collection.Where((x, i) => i == idx).First();
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> collection, int count)
        {
            var result = new List<T>();
            var collectionTemp = collection.ToList();
            for (int i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, collectionTemp.Count);
                result.Add(collectionTemp[randomIndex]);
                collectionTemp.RemoveAt(randomIndex);
            }

            return result;
        }

    }
}