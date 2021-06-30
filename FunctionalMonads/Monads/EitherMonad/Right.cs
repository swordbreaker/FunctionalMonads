namespace FunctionalMonads.Monads.EitherMonad
{
    public interface Right<out TLeft, out TRight> : IEither<TLeft, TRight>, Right<TRight>
    {
    }

    public interface Right<out TRight>
    {
        TRight Value { get; }
    }
}