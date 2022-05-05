using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FunctionalMonads.Monads.EitherMonad;

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
        /// Convert a <see cref="IMaybe{T}"/> monad to a <see cref="IEither{TLeft,TRight}"/> monad.
        /// When the maybe is some return the value as a left value.
        /// Else return the provided right value.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="rightValue"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <returns></returns>
        public static IEither<TLeft, TRight> ToEitherLeft<TLeft, TRight>(this IMaybe<TLeft> self, TRight rightValue) =>
            self.Match(
                Either.Left<TLeft, TRight>,
                () => Either.Right<TLeft, TRight>(rightValue));

        /// <summary>
        /// Convert a <see cref="IMaybe{T}"/> monad to a <see cref="IEither{TLeft,TRight}"/> monad.
        /// When the maybe is some return the value as a left value.
        /// Else return the provided right value.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="leftValue"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <returns></returns>
        public static IEither<TLeft, TRight> ToEitherRight<TLeft, TRight>(this IMaybe<TRight> self, TLeft leftValue) =>
            self.Match(
                Either.Right<TLeft, TRight>,
                () => Either.Left<TLeft, TRight>(leftValue));
        
        /// <summary>
        /// Extracts from a list of `Option` all the `MaybeSome` elements.
        /// All the `MaybeSome` elements are extracted in order.
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

            return self.OfType<MaybeSome<T>>().Select(x => x.Value);
        }

        private static IMaybeT<T> ToMaybeT<T>(this IMaybe<T> self) =>
            self.Match<IMaybeT<T>>(
                t => new MaybeSome<T>(t),
                () => new MaybeNone<T>());

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