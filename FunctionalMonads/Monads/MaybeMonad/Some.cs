namespace FunctionalMonads.Monads.MaybeMonad
{
    public interface Some<out T> : IMaybe<T>
    {
        public T Value { get; }
    }
}