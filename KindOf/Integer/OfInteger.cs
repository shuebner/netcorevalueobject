using System;

namespace KindOf
{
    public class OfInteger<T> : Of<int>, IComparable<T>
        where T : OfInteger<T>
    {
        public OfInteger(int value) : base(value)
        {
        }

        public int CompareTo(T other) => Value.CompareTo(other.Value);

        public static bool operator ==(OfInteger<T> one, OfInteger<T> other) => one.Value == other.Value;

        public static bool operator !=(OfInteger<T> one, OfInteger<T> other) => one.Value != other.Value;
    }
}
