using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public interface IEitherT<TLeft, TRight> : IEither<TLeft, TRight>
    {
        public IEither<TBindLeft, TRight> BindLeft<TBindLeft>(Func<TLeft, IEither<TBindLeft, TRight>> bindFunc) =>
            Bind(bindFunc, Either.Right<TBindLeft, TRight>);

        public IEither<TLeft, TBindRight> BindRight<TBindRight>(Func<TRight, IEither<TLeft, TBindRight>> bindFunc) =>
            Bind(Either.Left<TLeft, TBindRight>, bindFunc);

        public IEither<TResult, TRight> SelectMany<TIntermediate, TResult>(
            Func<TLeft, IEitherT<TIntermediate, TRight>> mapper,
            Func<TLeft, TIntermediate, TResult> getResult) =>
            this.BindLeft(value =>
                mapper(value).BindLeft(
                    intermediate =>
                        Either.Left<TResult, TRight>(getResult(value, intermediate))));
    }
}