namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IPResult<out T>
    {
        T Value { get; }

        
    }
}