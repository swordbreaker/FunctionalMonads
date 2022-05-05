using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalMonads.Monads.MaybeMonad;

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
        /// Return all left values in the sequence.
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TLeft> OnlyLeft<TLeft, TRight>(this IEnumerable<IEither<TLeft, TRight>> self) =>
            self.OfType<Left<TLeft>>().Select(left => left.Value);
        
        /// <summary>
        /// Return all right values of the sequence.
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TRight> OnlyRight<TLeft, TRight>(this IEnumerable<IEither<TLeft, TRight>> self) =>
            self.OfType<Right<TRight>>().Select(right => right.Value);

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

        public static EitherLeft<TLeft> ToLeft<TLeft>(this TLeft left) => new(left);
        
        public static EitherRight<TRight> ToRight<TRight>(this TRight left) => new(left);

        public static IMaybe<TLeft> MaybeLeft<TLeft, TRight>(IEither<TLeft, TRight> either) =>
            either.Match(Maybe.Some, _ => Maybe.None<TLeft>());
        
        public static IMaybe<TRight> MaybeRight<TLeft, TRight>(IEither<TLeft, TRight> either) =>
            either.Match(_ => Maybe.None<TRight>(), Maybe.Some);
    }
}