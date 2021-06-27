using System;

namespace FunctionalMonads.Monads.EitherMonad
{
    public interface IEither<out TLeft, out TRight>
    {
        bool IsLeft { get; }

        bool IsRight { get; }

        TLeft LeftUnsafe { get; }

        TRight RightUnsafe { get; }

        IEither<TMapLeft, TMapRight> Map<TMapLeft, TMapRight>(Func<TLeft, TMapLeft> mapLeftFunc,
            Func<TRight, TMapRight> mapRightFunc);

        public IEither<TMapLeft, TRight> MapLeft<TMapLeft>(Func<TLeft, TMapLeft> mapLeftFunc)
            => Map(mapLeftFunc, Functional.Identity);

        public IEither<TLeft, TMapRight> MapRight<TMapRight>(Func<TRight, TMapRight> mapRightFunc) =>
            Map(Functional.Identity, mapRightFunc);

        IEither<TBindLeft, TBindRight> Bind<TBindLeft, TBindRight>(
            Func<TLeft, IEither<TBindLeft, TBindRight>> bindLeftFunc,
            Func<TRight, IEither<TBindLeft, TBindRight>> bindRightFunc);

        TRet Match<TRet>(Func<TLeft, TRet> onLeft, Func<TRight, TRet> onRight);

        Unit IfLeft(Action<TLeft> onLeft);

        Unit IfRight(Action<TRight> onRight);

        Unit Do(Action<TLeft> onLeft, Action<TRight> onRight);
    }
}