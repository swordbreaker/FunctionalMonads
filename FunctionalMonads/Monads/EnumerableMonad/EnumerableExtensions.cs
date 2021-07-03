using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalMonads.Monads.EnumerableMonad
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Match the head (first element) and the tail (all elements except the first).
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="head">First element.</param>
        /// <param name="tail">All elements except the first.</param>
        /// <returns></returns>
        public delegate TSelect HeadTailMatch<in T, out TSelect>(T head, IEnumerable<T> tail);

        public delegate TSelect InitLastMatch<in T, out TSelect>(IEnumerable<T> init, T last);

        public static IEnumerable<TMap> Map<T, TMap>(this IEnumerable<T> self, Func<T, TMap> mapFunc) =>
            self.Select(mapFunc);

        public static T Head<T>(this IEnumerable<T> self) =>
            self.First();

        public static IEnumerable<T> Tail<T>(this IEnumerable<T> self) =>
            self.Skip(1);

        public static IEnumerable<T> Init<T>(this IEnumerable<T> self) =>
            self.Take(self.Count() - 2);

        public static TSelect Match<T, TSelect>(this IEnumerable<T> self, HeadTailMatch<T, TSelect> headTailMatch, Func<TSelect> onEmpty)
        {
            var enumerator = self.GetEnumerator();

            if (enumerator.MoveNext())
            {
                var body = Enumerable.Empty<T>();
                var head = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    body = body.Append(enumerator.Current);
                }

                return headTailMatch(head, body);
            }

            enumerator.Dispose();
            return onEmpty();
        }

        public static TSelect Match<T, TSelect>(this IEnumerable<T> self, InitLastMatch<T, TSelect> initLastMatch, Func<TSelect> onEmpty)
        {
            var enumerator = self.GetEnumerator();

            if (enumerator.MoveNext())
            {
                var init = Enumerable.Empty<T>();
                var last = default(T);
                var isLast = false;

                while (!isLast)
                {
                    var current = enumerator.Current;
                    isLast = !enumerator.MoveNext();

                    if (isLast)
                    {
                        last = current;
                    }
                    else
                    {
                        init = init.Append(current);
                    }
                }

                return initLastMatch(init, last);
            }

            enumerator.Dispose();
            return onEmpty();
        }
    }
}