using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public class Left<TLeft, TRight> : IEitherT<TLeft, TRight>
    {
        public Left(TLeft leftValue)
        {
            LeftValue = leftValue;
        }


        public TLeft LeftValue { get; }
        public bool IsLeft => true;
        public bool IsRight => false;
        public TLeft LeftUnsafe => LeftValue;
        public TRight RightUnsafe => throw new NullReferenceException("Left has no right value.");

        public IEither<TMapLeft, TMapRight> Map<TMapLeft, TMapRight>(Func<TLeft, TMapLeft> mapLeftFunc, Func<TRight, TMapRight> mapRightFunc)
        {

            return Either.Left<TMapLeft, TMapRight>(mapLeftFunc(LeftValue));
        }

        public IEither<TBindLeft, TBindRight> Bind<TBindLeft, TBindRight>(Func<TLeft, IEither<TBindLeft, TBindRight>> bindLeftFunc, Func<TRight, IEither<TBindLeft, TBindRight>> bindRightFunc)
        {

            return bindLeftFunc(LeftValue);
        }

        public TRet Match<TRet>(Func<TLeft, TRet> onLeft, Func<TRight, TRet> onRight)
        {
            return onLeft(LeftValue);
        }

        public Unit IfLeft(Action<TLeft> onLeft)
        {
            onLeft(LeftValue);
            return new Unit();
        }

        public Unit IfRight(Action<TRight> onRight) => 
            new();

        public Unit Do(Action<TLeft> onLeft, Action<TRight> onRight) => 
            IfLeft(onLeft);

        public override string ToString() => 
            $"Left({LeftValue})";
    }
}