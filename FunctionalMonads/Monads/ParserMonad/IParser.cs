using System;
using FunctionalMonads.Monads.EitherMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParser<out T>
    {
        /// <summary>
        /// Parse the Text point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Either on success: <see cref="IPResult{T}"/> or on failure: <see cref="IParseFailure"/>.</returns>
        IEither<IPResult<T>, IParseFailure> Parse(TextPoint point);

        IParser<TMap> Map<TMap>(Func<IPResult<T>, TMap> mapFunc);

        IParser<TBind> Bind<TBind>(Func<IPResult<T>, IParser<TBind>> binFunc);

        public static IParser<T> operator |(IParser<T> a, IParser<T> b) =>
            Consume.Or(a, b);

        public static IParser<T> operator &(IParser<T> a, IParser<T> b) =>
            a.Then(b);

        public static IParser<Unit> operator !(IParser<T> parser) =>
            parser.Not("Invalid input.");
    }
}