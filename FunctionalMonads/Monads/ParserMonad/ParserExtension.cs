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

        public static IEither<T, IParseFailure> ParseToValue<T>(this IParser<T> self, string text) =>
        self.Parse(TextPoint.Create(text)).MapLeft(x => x.Value);

        public static string AsString(this IEnumerable<char> self) =>
            new(self.ToArray());

        public static IParser<TMap> Map<T, TMap>(this IParser<T> parser, Func<T, TMap> mapFunc) =>
            parser.Map(result => mapFunc(result.Value));

        public static IParser<TMap> Select<T, TMap>(this IParser<T> parser, Func<T, TMap> mapFunc) =>
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

        public static IParser<IEnumerable<T>> OneOrMore<T>(this IParser<T> parser) =>
            from one in parser
            from more in parser.Many()
            select more.Prepend(one);

        //public static IParser<TResult> SelectMany<TSource, TSelector, TResult>(
        //    this IParser<TSource> self,
        //    Func<IPResult<TSource>, IParser<TSelector>> selector,
        //    Func<IPResult<TSource>, TSelector, TResult> getResult) =>
        //    self.Bind(value =>
        //        selector(value).Map(
        //            intermediate =>
        //                getResult(value, intermediate)));


        public static IParser<TResult> SelectMany<TSource, TSelector, TResult>(
            this IParser<TSource> self,
            Func<TSource, IParser<TSelector>> selector,
            Func<TSource, TSelector, TResult> getResult) =>
            self.Bind(value =>
                selector(value.Value).Map(
                    intermediate =>
                        getResult(value.Value, intermediate)));

        //TMonad<TResult> SelectMany<TSource, TSelector, TResult>(
        //    TMonad<TSource> source,
        //    Func<TSource, TMonad<TSelector>> selector,
        //    Func<TSource, TSelector, TResult> resultSelector);

        //public static IParser<B> SelectMany<A, B>(
        //    this IParser<A> first,
        //    Func<IPResult<A>, IParser<B>> selector)
        //    => first.Bind(selector)


        public static IParser<IMaybe<T>> Optional<T>(this IParser<T> self) =>
            new Parser<IMaybe<T>>(point =>
                self.Parse(point).Bind(
                    result => Either.Left<IPResult<IMaybe<T>>, IParseFailure>(
                        result.With(Maybe.Some(result.Value))),
                    failure => Either.Left<IPResult<IMaybe<T>>, IParseFailure>(
                        new PResult<IMaybe<T>>(Maybe.None<T>(), failure.Start, failure.End))));

        public static IEither<IPResult<T>, IParseFailure> WithNewStart<T>(this IEither<IPResult<T>, IParseFailure> self, TextPoint start) =>
            self.Map(x => x.With(start, x.Next), x => x.With(start, x.End));

        //public static IParser<T> Token<T>(IParser<T> self) =>
        //    from head in Consume.Whitespace.Many()
        //    from content in self
        //    from tail in Consume.Whitespace.Many()
        //    select content;

        public static IParser<T> Token<T>(this IParser<T> self) =>
            from head in Consume.Whitespace.Many()
            from content in self
            from tail in Consume.Whitespace.Many()
            select content;
    }
}