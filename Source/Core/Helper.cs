using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace Pamola
{
    public static class Helper
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when <paramref name="source"/> is null.
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> type.</typeparam>
        /// <param name="source">Analysed object.</param>
        /// <param name="parameterName">Local name of <paramref name="source"/>.</param>
        /// <returns></returns>
        public static T ThrowOnNull<T>(this T source,string parameterName = "")
        {
            if (source==null) throw new ArgumentNullException(parameterName);            
            return source;
        }

        public static IEnumerable<T> ToCachedEnumerable<T>(
            this IEnumerable<T> source
        )
        {
            var cache = new List<T>();
            return source.GetEnumerator().ToCachedEnumerableHelper(cache);
        }

        private static IEnumerable<T> ToCachedEnumerableHelper<T>(
            this IEnumerator<T> source,
            IList<T> cache
        )
        {
            foreach (var t in cache)
            {
                yield return t;
            }

            while(source.MoveNext())
            {
                cache.Add(source.Current);
                yield return source.Current;
            }
        }
    }
}
