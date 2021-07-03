namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParseFailure
    {
        TextPoint Point { get; }

        string Message { get; }
    }
}