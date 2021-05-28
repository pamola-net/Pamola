﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Pamola.Transient;

namespace Pamola
{
    /// <summary>
    /// Provides generic helper extension methods.
    /// </summary>
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

        /// <summary>
        /// Caches an Enumerable, ensuring that each item on the <paramref name="source"/> is called only once. 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ToCachedEnumerable<T>(
            this IEnumerable<T> source
        )
        {
            var cache = new List<T>();
            return source.GetEnumerator().ToCachedEnumerableHelper(cache);
        }

        /// <summary>
        /// Internal logic for <see cref="ToCachedEnumerable"/>. The first time an item is yielded,
        /// store its value in <paramref name="cache"/>. If an item is called again, return the
        /// cached value.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cache"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Set the values of all <see cref="TransientVariable"/>'s in a <paramref name="circuit"/>,
        /// according to <paramref name="stateVariables"/>.
        /// </summary>
        /// <param name="circuit">An electric circuit.</param>
        /// <param name="stateVariables">A state response.</param>
        public static void SetTransientVariables(
            this Circuit circuit, 
            IEnumerable<Complex> stateVariables) => 
                stateVariables.Zip(
                    circuit.GetTransientVariables(),
                    (s, v) => (State: s, TransientVariable: v)).ToList()
                    .ForEach(sv => sv.TransientVariable.Variable.Setter(sv.State));
        
    }
}
