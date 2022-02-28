namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParserOutput
    {
        TextPoint Start { get; }
        TextPoint Next { get; }
    }
}
