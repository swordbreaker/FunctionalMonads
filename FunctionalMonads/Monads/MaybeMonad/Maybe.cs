namespace FunctionalMonads.Monads.MaybeMonad
{
    /// <summary>
    /// Static maybe methods.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Create some from a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>A new maybe.</returns>
        public static IMaybe<T> Some<T>(T value) => new MaybeSome<T>(value);

        /// <summary>
        /// Create a none of a specific inner type.
        /// </summary>
        /// <typeparam name="T">The inner type.</typeparam>
        /// <returns>A new maybe.</returns>
        public static IMaybe<T> None<T>() => new MaybeNone<T>();

        /// <summary>
        /// Convert a nullable class to a maybe.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="nullableValue">The nullable value.</param>
        /// <returns><c>MaybeSome</c> when nullableValue is not null; else <c>MaybeNone</c>.</returns>
        public static IMaybe<T> ToMaybe<T>(T nullableValue) =>
            nullableValue is null
                ? None<T>()
                : Some(nullableValue);

        /// <summary>
        /// Convert a nullable struct to a maybe.
        /// </summary>
        /// <typeparam name="T">The type of the struct.</typeparam>
        /// <param name="nullableValue">The nullable value.</param>
        /// <returns><c>MaybeSome</c> when nullableValue is not null; else <c>MaybeNone</c>.</returns>
        public static IMaybe<T> ToMaybe<T>(T? nullableValue)
            where T : struct =>
            nullableValue.HasValue
                ? Some(nullableValue.Value)
                : None<T>();
    }
}