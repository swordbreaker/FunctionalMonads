using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FunctionalMonads.Monads.MaybeMonad
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Monadic join. Flatten two nested Maybes to one.
        /// </summary>
        /// <typeparam name="T">The type value of the inner maybe.</typeparam>
        /// <param name="maybe">The nested maybe.</param>
        /// <returns>One Maybe instead of two nested ones.</returns>
        public static IMaybe<T> Flatten<T>(this IMaybe<IMaybe<T>> maybe) =>
            maybe.Bind(Functional.Identity);

        /// <summary>
        /// Get some value of a dictionary by key if key exists else none.
        /// </summary>
        /// <typeparam name="TValue">Type of the dictionary values.</typeparam>
        /// <typeparam name="TKey">Type of the dictionary keys.</typeparam>
        /// <param name="self">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>Maybe a value.</returns>
        public static IMaybe<TValue> GetMaybe<TValue, TKey>(this IDictionary<TKey, TValue> self, TKey key)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return self.ContainsKey(key)
                ? Maybe.Some(self[key])
                : Maybe.None<TValue>();
        }

        /// <summary>
        /// Convert to result.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFailure">The type of the failure.</typeparam>
        /// <param name="self">The maybe monad to convert.</param>
        /// <param name="onNone">Failure when the maybe is none.</param>
        /// <returns>A result which is success on some and failure on none.</returns>
        //public static IResult<TValue, TFailure> ToResult<TValue, TFailure>(this Maybe<TValue> self, TFailure onNone)
        //    where TFailure : Failure =>
        //        self.Match(
        //            some: Result.Success<TValue, TFailure>,
        //            none: Result.Failure<TValue, TFailure>(onNone));

        /// <summary>
        /// Extracts from a list of `Option` all the `Some` elements.
        /// All the `Some` elements are extracted in order.
        /// </summary>
        /// <typeparam name="TValue">The inner type of the Maybe.</typeparam>
        /// <param name="self">The sequence of Maybes.</param>
        /// <returns>Returns a list of T.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Somes<T>(this IEnumerable<IMaybe<T>> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return self.OfType<Some<T>>().Select(x => x.Value);
        }

        private static IMaybeT<T> ToMaybeT<T>(this IMaybe<T> self) =>
            self.IsSome 
                ? new Some<T>(self.SomeUnsafe) 
                : new None<T>();

        public static T SomeOrProvided<T>(this IMaybe<T> self, Func<T> provider) =>
            self.ToMaybeT().SomeOrProvided(provider);

        public static T SomeOrProvided<T>(this IMaybe<T> self, T provided) =>
            self.ToMaybeT().SomeOrProvided(provided);


        /// <summary>
        /// Keep the contained object when some else replace it with another maybe.
        /// </summary>
        /// <param name="bindFunction">The bind function to invoke on none.</param>
        /// <returns>This on some else the output of the bindFunction.</returns>
        public static IMaybe<T> BindNone<T>(this IMaybe<T> self, Func<IMaybe<T>> bindFunction) =>
            self.ToMaybeT().BindNone(bindFunction);

        public static IMaybe<T> BindNone<T>(IMaybe<T> noneMaybe) =>
            noneMaybe.ToMaybeT().BindNone(noneMaybe);
    }
}