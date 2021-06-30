namespace FunctionalMonads.Monads.EitherMonad
{
    public interface Left<out TLeft, out TRight> : IEither<TLeft, TRight>, Left<TLeft>
    {
        
    }

    public interface Left<out TLeft>
    {
        TLeft Value { get; }
    }
}