using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public static class ParserExtension
    {
        public static IEither<IPResult<T>, IParseFailure> Parse<T>(this IParser<T> self, string text) =>
            self.Parse(TextPoint.Create(text));

        public static string AsString(this IEnumerable<char> self) =>
            new(self.ToArray());

        public static IParser<TMap> Map<T, TMap>(this IParser<T> parser, Func<T, TMap> mapFunc) =>
            parser.Map(result => mapFunc(result.Value));

        public static IParser<TResult> Then<T, TResult>(this IParser<T> self, IParser<TResult> parser) =>
            self.Bind(_ => parser);

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
                    new PResult<IEnumerable<T>>(elements, point, current));
            });
        }

        public static IParser<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this IParser<TSource> self,
            Func<IPResult<TSource>, IParser<TIntermediate>> mapper,
            Func<IPResult<TSource>, IPResult<TIntermediate>, TResult> getResult) =>
            self.Bind(value =>
                mapper(value).Map(
                    intermediate =>
                        getResult(value, intermediate)));

        public static IParser<IMaybe<T>> Optional<T>(this IParser<T> self) =>
            new Parser<IMaybe<T>>(point =>
                self.Parse(point).Bind(
                    result => Either.Left<IPResult<IMaybe<T>>, IParseFailure>(
                        result.With(Maybe.Some(result.Value))),
                    failure => Either.Left<IPResult<IMaybe<T>>, IParseFailure>(
                        new PResult<IMaybe<T>>(Maybe.None<T>(), failure.Start, failure.End))));

        public static IParser<T> Token<T>(IParser<T> self) =>
            from head in Consume.Whitespace.Many()
            from content in self
            from tail in Consume.Whitespace.Many()
            select 
    }
}