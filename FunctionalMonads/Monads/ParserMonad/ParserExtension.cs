using System.Collections.Generic;
using System.Linq;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public static class ParserExtension
    {
        public static string AsString(this IEnumerable<char> self) =>
            new(self.ToArray());

        public static IParser<IEnumerable<T>> Many<T>(this IParser<T> parser)
        {
            return new Parser<IEnumerable<T>>(point =>
            {
                var elements = new List<T>();
                var current = point;
                var result = parser.Parse(point);
                while (current.Advance() is Some<TextPoint> next &&
                       result is Left<IPResult<T>, IParseFailure> successElement)
                {
                    elements.Add(successElement.Value.Value);
                    current = next.Value;
                    result = parser.Parse(current);
                }

                return Either.Left<IPResult<IEnumerable<T>>, IParseFailure>(
                    new PResult<IEnumerable<T>>(elements));
            });
        }
            
    }
}