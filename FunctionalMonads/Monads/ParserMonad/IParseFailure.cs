namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParseFailure
    {
        TextPoint Start { get; }
        TextPoint End { get; }

        string Message { get; }
    }
}