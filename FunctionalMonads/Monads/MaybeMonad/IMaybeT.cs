using System;

namespace FunctionalMonads.Monads.MaybeMonad
{
    /// <summary>
    /// Maybe type without contravariance, but with more functionality.
    /// </summary>
    /// <typeparam name="T">Inner monad type.</typeparam>
    internal interface IMaybeT<T> : IMaybe<T>, IEquatable<IMaybe<T>>
    {
        T SomeOrProvided(Func<T> provider);

        public T SomeOrProvided(T provided) =>
            SomeOrProvided(() => provided);

        /// <summary>
        /// Keep the contained object when some else replace it with another maybe.
        /// </summary>
        /// <param name="bindFunction">The bind function to invoke on none.</param>
        /// <returns>This on some else the output of the bindFunction.</returns>
        public IMaybe<T> BindNone(Func<IMaybe<T>> bindFunction);

        public IMaybe<T> BindNone(IMaybe<T> noneMaybe) =>
            this.BindNone(() => noneMaybe);
    }
}