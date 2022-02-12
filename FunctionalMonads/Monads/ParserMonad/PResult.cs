using System;

namespace FunctionalMonads.Monads.ParserMonad
{
    public record PResult<T>(T Value, TextPoint Start, TextPoint End) : IPResult<T>
    {
        public IPResult<K> With<K>(K newValue) =>
            new PResult<K>(newValue, Start, End);

        public override string ToString() =>
            $"{Value} from {Start} to {End}";

        public IPResult<T> With(TextPoint start, TextPoint end) => 
            this with { Start = start, End = end };
    }
}