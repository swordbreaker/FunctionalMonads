using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalMonads.Monads.MaybeMonad
{
    public static class EnumerableExtention
    {
        public static IMaybe<T> FirstOrNone<T>(this IEnumerable<T> self, Func<T, bool> predicate) where T : class =>
            Maybe.ToMaybe(self.FirstOrDefault(predicate));

        public static IMaybe<T> FirstOrNone<T>(this IEnumerable<T> self) where T : class =>
            Maybe.ToMaybe(self.FirstOrDefault());
    }
}
