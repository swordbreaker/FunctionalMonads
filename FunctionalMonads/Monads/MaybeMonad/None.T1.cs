using System;

namespace FunctionalMonads.Monads.MaybeMonad
{
    public readonly struct None<T> : IMaybeT<T>
    {
        public bool IsSome => false;
        public bool IsNone => true;
        public T SomeUnsafe => throw new NullReferenceException();

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

        public static bool operator ==(None<T> maybe, T value) => 
            false;

        public static bool operator !=(None<T> maybe, T value) =>
            !(maybe == value);

        public bool Equals(IMaybe<T>? other) => 
            other is None<T>;

        public bool Equals(None<T> other) => 
            true;

        public override bool Equals(object obj) => 
            obj is None<T> other && Equals(other);

        public override int GetHashCode() => 
            typeof(T).GetHashCode();

        public override string ToString() =>
            "None()";
    }
}