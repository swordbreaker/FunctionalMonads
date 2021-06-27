using System;

namespace FunctionalMonads.Monads.MaybeMonad
{
    /// <summary>
    /// Monad which represent a value which can have some value or none value.
    /// </summary>
    /// <typeparam name="T">Inner contravariance monad type.</typeparam>
    public interface IMaybe<out T>
    {
        bool IsSome { get; }
        
        bool IsNone { get; }

        T SomeUnsafe { get; }

        IMaybe<TMap> Map<TMap>(Func<T, TMap> mapFunc);

        IMaybe<TBind> Bind<TBind>(Func<T, IMaybe<TBind>> binFunc);

        public IMaybe<TResult> SelectMany<TIntermediate, TResult>(
            Func<T, IMaybe<TIntermediate>> mapper,
            Func<T, TIntermediate, TResult> getResult) =>
            this.Bind(value =>
                mapper(value).Bind(
                    intermediate =>
                        Maybe.Some(getResult(value, intermediate))));

        TRet Match<TRet>(Func<T, TRet> onSome, Func<TRet> onNone);

        Unit IfSome(Action<T> onSome);

        Unit IfNone(Action onNone);

        Unit Do(Action<T> onSome, Action onNone);
    }
}