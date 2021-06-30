using System;

namespace FunctionalMonads.Monads
{
    public static class ActionExtension
    {
        public static Func<T, Unit> ToUnit<T>(this Action<T> a) =>
            t =>
            {
                a(t);
                return new Unit();
            };

        public static Func<Unit> ToUnit(this Action a) =>
            () =>
            {
                a();
                return new Unit();
            };

        public static Action<K> Currying<T, K>(this Action<T, K> action, T value) =>
            k => action(value, k);

        public static Action<K, U> Currying<T, K, U>(this Action<T, K, U> action, T value) =>
            (k, u) => action(value, k, u);

        public static Action<U> Currying<T, K, U>(this Action<T, K, U> action, T tValue, K kValue) =>
            u => action(tValue, kValue, u);
    }
}