namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParseFailure : IParserOutput
    {
        string Message { get; }

        IParseFailure With(TextPoint start, TextPoint end);
    }
}