namespace FunctionalMonads.Monads.MaybeMonad
{
    public interface None<out T> : IMaybe<T>, None
    {
    }

    public interface None
    {
    }
}