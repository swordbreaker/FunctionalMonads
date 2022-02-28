namespace FunctionalMonads.Monads.ParserMonad
{
    public record PResult<T>(T Value, TextPoint Start, TextPoint Next) : IPResult<T>
    {
        public IPResult<K> With<K>(K newValue) =>
            new PResult<K>(newValue, Start, Next);

        public override string ToString() =>
            $"{Value} from {Start} to {Next}";

        public IPResult<T> With(TextPoint start, TextPoint end) => 
            this with { Start = start, Next = end };
    }
}