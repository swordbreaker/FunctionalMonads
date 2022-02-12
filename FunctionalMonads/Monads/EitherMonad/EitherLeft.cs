using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public record EitherLeft<TLeft, TRight> : IEitherT<TLeft, TRight>, Left<TLeft, TRight>
    {
        public EitherLeft(TLeft value)
        {
            Value = value;
        }

        public TLeft Value { get; }
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

        public override string ToString() => 
            $"EitherLeft({Value})";
    }
}