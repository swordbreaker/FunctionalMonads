using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public static class EitherExtensions
    {
        private static IEitherT<TLeft, TRight> ToEitherT<TLeft, TRight>(this IEither<TLeft, TRight> self) =>
            self.Match<IEitherT<TLeft, TRight>>(
                left => new EitherLeft<TLeft, TRight>(left),
                right => new EitherRight<TLeft, TRight>(right));

        public static IEither<TBindLeft, TRight> BindLeft<TLeft, TRight, TBindLeft>(this IEither<TLeft, TRight> self, Func<TLeft, IEither<TBindLeft, TRight>> bindFunc) =>
            self.Bind(bindFunc, Either.Right<TBindLeft, TRight>);

        public static IEither<TLeft, TBindRight> BindRight<TLeft, TRight, TBindRight>(this IEither<TLeft, TRight> self, Func<TRight, IEither<TLeft, TBindRight>> bindFunc) =>
            self.Bind(Either.Left<TLeft, TBindRight>, bindFunc);

        /// <summary>
        /// This allows the form .. in .. syntax.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TIntermediate"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="mapper"></param>
        /// <param name="getResult"></param>
        /// <returns></returns>
        public static IEither<TResult, TRight> SelectMany<TLeft, TRight, TIntermediate, TResult>(
            this IEither<TLeft, TRight> self,
            Func<TLeft, IEitherT<TIntermediate, TRight>> mapper,
            Func<TLeft, TIntermediate, TResult> getResult) =>
            self.BindLeft(value =>
                mapper(value).BindLeft(
                    intermediate =>
                        Either.Left<TResult, TRight>(getResult(value, intermediate))));
    }
}