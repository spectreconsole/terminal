using System;
using System.Collections.Generic;

namespace Spectre.Terminal
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(bool First, bool Last, T Item)> Enumerate<T>(this IEnumerator<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var first = true;
            var last = !source.MoveNext();
            T current;

            for (var index = 0; !last; index++)
            {
                current = source.Current;
                last = !source.MoveNext();
                yield return (first, last, current);
                first = false;
            }
        }
    }
}
