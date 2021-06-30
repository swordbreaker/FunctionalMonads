using System;

namespace FunctionalMonads.Monads.MaybeMonad
{
    public readonly struct MaybeNone<T> : None<T>, IMaybeT<T>
    {
        public bool IsSome => false;
        public bool IsNone => true;

        public IMaybe<TMap> Map<TMap>(Func<T, TMap> mapFunc) =>
            Maybe.None<TMap>();

        public IMaybe<TBind> Bind<TBind>(Func<T, IMaybe<TBind>> binFunc) => 
            Maybe.None<TBind>();

        public TRet Match<TRet>(Func<T, TRet> onSome, Func<TRet> onNone) => 
            onNone();

        public Unit IfSome(Action<T> onSome) =>
            new();

        public Unit IfNone(Action onNone)
        {
            onNone();
            return new Unit();
        }

        public Unit Do(Action<T> onSome, Action onNone)
        {
            onNone();
            return new Unit();
        }

        public T SomeOrProvided(Func<T> provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            return provider();
        }

        public IMaybe<T> BindNone(Func<IMaybe<T>> bindFunction)
        {
            if(bindFunction == null) throw new ArgumentNullException(nameof(bindFunction));
            return bindFunction();
        }

        public static bool operator ==(MaybeNone<T> maybe, T value) => 
            false;

        public static bool operator !=(MaybeNone<T> maybe, T value) =>
            !(maybe == value);

        public bool Equals(IMaybe<T>? other) => 
            other is MaybeNone<T>;

        public bool Equals(MaybeNone<T> other) => 
            true;

        public override bool Equals(object obj) => 
            obj is MaybeNone<T> other && Equals(other);

        public override int GetHashCode() => 
            typeof(T).GetHashCode();

        public override string ToString() =>
            "MaybeNone()";
    }
}