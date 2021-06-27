using System;

namespace FunctionalMonads.Monads
{
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>
    {
        /// <inheritdoc/>
        public override string ToString() =>
            "()";

        /// <inheritdoc/>
        public override bool Equals(object obj) => 
            obj is Unit other && Equals(other);

        /// <summary>
        /// Check if two units are equal.
        /// </summary>
        /// <param name="other">The other unit value.</param>
        /// <returns>Returns always true.</returns>
        public bool Equals(Unit other) =>
            true;

        /// <summary>
        /// Always equal.
        /// </summary>
        /// <param name="other">The other unit value.</param>
        /// <returns>Always zero.</returns>
        public int CompareTo(Unit other) =>
            0;

        public override int GetHashCode() =>
            0;

        public static implicit operator ValueTuple(Unit _) =>
            default;

        public static implicit operator Unit(ValueTuple _) =>
            default;

        public static bool operator ==(Unit a, Unit b) =>
            true;

        public static bool operator !=(Unit a, Unit b) =>
            false;

        public static bool operator >(Unit a, Unit b) =>
            false;

        public static bool operator >=(Unit a, Unit b) =>
            true;

        public static bool operator <(Unit a, Unit b) =>
            false;

        public static bool operator <=(Unit a, Unit b) =>
            true;

        public static Unit operator +(Unit a, Unit bb) =>
            new();
    }
}