namespace FunctionalMonads.Monads.ParserMonad
{
    public class ParseFailure : IParseFailure
    {
        public TextPoint Point { get; }
        public string Message { get; }

        public ParseFailure(TextPoint point, string message)
        {
            Point = point;
            Message = message;
        }
    }
}