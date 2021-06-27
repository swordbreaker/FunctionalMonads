namespace FunctionalMonads.Monads.EitherMonad
{
    /// <summary>
    /// Static Either methods.
    /// </summary>
    public static class Either
    {
        public static IEither<TLeft, TRight> Left<TLeft, TRight>(TLeft left) =>
            new Left<TLeft, TRight>(left);

        public static IEither<TLeft, TRight> Right<TLeft, TRight>(TRight right) =>
            new Right<TLeft, TRight>(right);
    }
}