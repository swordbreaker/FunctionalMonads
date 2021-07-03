namespace FunctionalMonads.Monads.ParserMonad
{
    public class PResult<T> : IPResult<T>
    {
        public T Value { get; }
    }
}