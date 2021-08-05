namespace FunctionalMonads.Monads.ParserMonad
{
    public record ParseFailure(TextPoint Start, TextPoint End, string Message) : IParseFailure
    {
    }
}