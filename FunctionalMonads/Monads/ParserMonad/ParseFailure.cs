namespace FunctionalMonads.Monads.ParserMonad
{
    public record ParseFailure(TextPoint Start, TextPoint Next, string Message) : IParseFailure
    {
        public IParseFailure With(TextPoint start, TextPoint next) => this with { Start = start, Next = next };
    }
}