using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public class Right<TLeft, TRight> : IEitherT<TLeft, TRight>
    {
        public Right(TRight rightValue)
        {
            RightValue = rightValue;
        }

        public TRight RightValue { get; }
        public bool IsLeft => false;
        public bool IsRight => true;
        public TLeft LeftUnsafe => throw new NullReferenceException("Right has no left value.");
        public TRight RightUnsafe => RightValue;

        public IEither<TMapLeft, TMapRight> Map<TMapLeft, TMapRight>(Func<TLeft, TMapLeft> mapLeftFunc, Func<TRight, TMapRight> mapRightFunc)
        {

            return Either.Right<TMapLeft, TMapRight>(mapRightFunc(RightValue));
        }

        public IEither<TBindLeft, TBindRight> Bind<TBindLeft, TBindRight>(Func<TLeft, IEither<TBindLeft, TBindRight>> bindLeftFunc, Func<TRight, IEither<TBindLeft, TBindRight>> bindRightFunc)
        {

            return bindRightFunc(RightValue);
        }

        public TRet Match<TRet>(Func<TLeft, TRet> onLeft, Func<TRight, TRet> onRight)
        {

            return onRight(RightValue);
        }

        public Unit IfLeft(Action<TLeft> onLeft) => 
            new();

        public Unit IfRight(Action<TRight> onRight)
        {
            onRight(RightValue);
            return new Unit();
        }

        public Unit Do(Action<TLeft> onLeft, Action<TRight> onRight) => 
            IfRight(onRight);

        public override string ToString() =>
            $"Right({RightValue})";
    }
}