namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParseFailure
    {
        TextPoint Start { get; }
        TextPoint End { get; }

        string Message { get; }

        IParseFailure With(TextPoint start, TextPoint end);
    }
}