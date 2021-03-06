using System;
using System.Collections.Generic;
using static FunctionalMonads.Monads.MaybeMonad.Maybe;

namespace FunctionalMonads.Monads.MaybeMonad
{
    public readonly struct MaybeSome<T> : Some<T>, IMaybeT<T>
    {
        public MaybeSome(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public bool IsSome => true;
        public bool IsNone => false;

        public IMaybe<TMap> Map<TMap>(Func<T, TMap> mapFunc)
        {
            if (mapFunc == null) throw new ArgumentNullException(nameof(mapFunc));
            return Some(mapFunc(Value));
        }

        public IMaybe<TBind> Bind<TBind>(Func<T, IMaybe<TBind>> binFunc)
        {
            if (binFunc == null) throw new ArgumentNullException(nameof(binFunc));
            return binFunc(Value);
        }

        public TRet Match<TRet>(Func<T, TRet> onSome, Func<TRet> onNone) => 
            onSome(Value);

        public Unit IfSome(Action<T> onSome)
        {
            onSome(Value);
            return new Unit();
        }

        public Unit IfNone(Action onNone) => 
            new();

        public Unit Do(Action<T> onSome, Action onNone)
        {
            onSome(Value);
            return new Unit();
        }

        public T SomeOrProvided(Func<T> provider) => Value;

        public IMaybe<T> BindNone(Func<IMaybe<T>> bindFunction) => 
            this;

        public static bool operator ==(MaybeSome<T> maybe, T value) => 
            maybe.Value.Equals(value);

        public static bool operator !=(MaybeSome<T> maybe, T value) =>
            !(maybe == value);

        public static implicit operator MaybeSome<T>(T value) =>
            new(value);

        public static implicit operator T(MaybeSome<T> maybeSome) =>
            maybeSome.Value;

        public bool Equals(MaybeSome<T> other) => 
            EqualityComparer<T>.Default.Equals(Value, other.Value);

        public bool Equals(IMaybe<T> other) => 
            Equals((object)other);

        public override bool Equals(object obj) => 
            obj is MaybeSome<T> other && Equals(other);

        public override int GetHashCode() => 
            EqualityComparer<T>.Default.GetHashCode(Value);

        public override string ToString() =>
            $"MaybeSome({this.Value})";
    }
}