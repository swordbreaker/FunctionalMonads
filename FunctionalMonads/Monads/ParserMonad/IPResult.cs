namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IPResult<out T> : IParserOutput
    {
        T Value { get; }

        IPResult<K> With<K>(K newValue);

        IPResult<T> With(TextPoint start, TextPoint end);
    }
}