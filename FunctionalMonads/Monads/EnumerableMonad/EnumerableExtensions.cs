using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalMonads.Monads.EnumerableMonad
{
    public static class EnumerableExtensions
    {
        public delegate IEnumerable<TSelect> HeadTailMatch<in T, out TSelect>(T head, IEnumerable<T> tail);

        public delegate IEnumerable<TSelect> InitLastMatch<in T, out TSelect>(IEnumerable<T> init, T last);

        public static IEnumerable<TMap> Map<T, TMap>(this IEnumerable<T> self, Func<T, TMap> mapFunc) =>
            self.Select(mapFunc);

        public static T Head<T>(this IEnumerable<T> self) =>
            self.First();

        public static IEnumerable<T> Tail<T>(this IEnumerable<T> self) =>
            self.Skip(1);

        public static IEnumerable<T> Init<T>(this IEnumerable<T> self) =>
            self.Take(self.Count() - 2);

        public static IEnumerable<TSelect> Match<T, TSelect>(this IEnumerable<T> self, HeadTailMatch<T, TSelect> headTailMatch)
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
            return Enumerable.Empty<TSelect>();
        }

        public static IEnumerable<TSelect> Match<T, TSelect>(this IEnumerable<T> self, InitLastMatch<T, TSelect> initLastMatch)
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
            return Enumerable.Empty<TSelect>();
        }
    }
}