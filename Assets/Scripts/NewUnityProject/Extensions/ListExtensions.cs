using System.Collections.Generic;
using UnityEngine;

namespace NewUnityProject.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
        
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--) {
                var j = Random.Range(0, i + 1);
                list.Swap(i, j);
            }
        }
    }
}