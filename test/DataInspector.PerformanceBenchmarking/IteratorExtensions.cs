using System;
using System.Collections.Generic;
using System.Linq;

namespace DataInspector.PerformanceBenchmarking {
    public static class IteratorExtensions {
        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) {
            var asArray = list.ToArray();

            // Shuffle the array first; we use a Fisher-Yates shuffle here.
            for (var i = asArray.Length - 1; i > 0; --i) {
                var j = Rand.Next(0, i + 1);
                var tmp = asArray[i];
                asArray[i] = asArray[j];
                asArray[j] = tmp;
            }

            return asArray;
        }
    }
}
