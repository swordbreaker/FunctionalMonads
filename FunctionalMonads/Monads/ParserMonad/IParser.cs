using System;
using FunctionalMonads.Monads.EitherMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public interface IParser<out T>
    {
        IEither<IPResult<T>, IParseFailure> Parse(TextPoint point);

        IParser<TMap> Map<TMap>(Func<IPResult<T>, TMap> mapFunc);

        IParser<TBind> Bind<TBind>(Func<IPResult<T>, IParser<TBind>> binFunc);

        public static IParser<T> operator |(IParser<T> a, IParser<T> b) =>
            Consume.Or(a, b);

        public static IParser<T> operator &(IParser<T> a, IParser<T> b) =>
            a.Then(b);

        public static IParser<Unit> operator !(IParser<T> parser) =>
            parser.Not("Invalid input.");

        //public IParser<TResult> SelectMany<TIntermediate, TResult>(
        //    Func<T, IParser<TIntermediate>> mapper,
        //    Func<T, TIntermediate, TResult> getResult) =>
        //    this.Bind(value =>
        //        mapper(value).Bind(
        //            intermediate =>
        //                Maybe.Some(getResult(value, intermediate))));
    }
}