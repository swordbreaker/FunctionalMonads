using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public record EitherLeft<TLeft>(TLeft Value)
    {
        public IEither<TLeft, TRight> WithRight<TRight>() =>
            (EitherLeft<TLeft, TRight>)this;
    }
    
    public record EitherLeft<TLeft, TRight>(TLeft Value) : IEitherT<TLeft, TRight>, Left<TLeft, TRight>
    {
        public bool IsLeft => true;
        public bool IsRight => false;

        public IEither<TMapLeft, TMapRight> Map<TMapLeft, TMapRight>(Func<TLeft, TMapLeft> mapLeftFunc, Func<TRight, TMapRight> mapRightFunc) => 
            Either.Left<TMapLeft, TMapRight>(mapLeftFunc(Value));

        public IEither<TBindLeft, TBindRight> Bind<TBindLeft, TBindRight>(Func<TLeft, IEither<TBindLeft, TBindRight>> bindLeftFunc, Func<TRight, IEither<TBindLeft, TBindRight>> bindRightFunc) => 
            bindLeftFunc(Value);

        public TRet Match<TRet>(Func<TLeft, TRet> onLeft, Func<TRight, TRet> onRight) => 
            onLeft(Value);

        public Unit Do(Action<TLeft> onLeft, Action<TRight> onRight) =>
            onLeft.ToUnit()(Value);

        public static implicit operator EitherLeft<TLeft, TRight>(EitherLeft<TLeft> other) =>
            new(other.Value);

        public override string ToString() => 
            $"EitherLeft({Value})";
    }
}