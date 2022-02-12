using System;
using System.Data;

namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IPResult<out T>
    {
        T Value { get; }

        TextPoint Start { get; }

        TextPoint Next { get; }

        IPResult<K> With<K>(K newValue);

        IPResult<T> With(TextPoint start, TextPoint end);
    }
}