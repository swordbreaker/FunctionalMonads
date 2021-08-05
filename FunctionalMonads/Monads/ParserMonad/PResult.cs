using System;

namespace FunctionalMonads.Monads.ParserMonad
{
    public class PResult<T> : IPResult<T>
    {
        public T Value { get; }
        public TextPoint Start { get; }
        public TextPoint End { get; }
        public IPResult<K> With<K>(K newValue) =>
            new PResult<K>(newValue, Start, End);

        public PResult(T value, TextPoint start, TextPoint end)
        {
            Value = value;
            Start = start;
            End = end;
        }
    }
}