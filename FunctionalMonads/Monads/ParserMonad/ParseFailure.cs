namespace FunctionalMonads.Monads.ParserMonad
{
    public record ParseFailure(TextPoint Start, TextPoint End, string Message) : IParseFailure
    {
        public IParseFailure With(TextPoint start, TextPoint end) => this with { Start = start, End = end };
    }
}