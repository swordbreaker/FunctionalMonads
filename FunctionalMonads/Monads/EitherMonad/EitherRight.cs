using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public class EitherRight<TLeft, TRight> : IEitherT<TLeft, TRight>, Right<TLeft, TRight>
    {
        public EitherRight(TRight value)
        {
            Value = value;
        }

        public TRight Value { get; }
        public bool IsLeft => false;
        public bool IsRight => true;

        public IEither<TMapLeft, TMapRight> Map<TMapLeft, TMapRight>(Func<TLeft, TMapLeft> mapLeftFunc, Func<TRight, TMapRight> mapRightFunc) => 
            Either.Right<TMapLeft, TMapRight>(mapRightFunc(Value));

        public IEither<TBindLeft, TBindRight> Bind<TBindLeft, TBindRight>(Func<TLeft, IEither<TBindLeft, TBindRight>> bindLeftFunc, Func<TRight, IEither<TBindLeft, TBindRight>> bindRightFunc) => 
            bindRightFunc(Value);

        public TRet Match<TRet>(Func<TLeft, TRet> onLeft, Func<TRight, TRet> onRight) => 
            onRight(Value);

        public Unit Do(Action<TLeft> onLeft, Action<TRight> onRight) =>
            onRight.ToUnit()(Value);

        public override string ToString() =>
            $"EitherRight({Value})";
    }
}