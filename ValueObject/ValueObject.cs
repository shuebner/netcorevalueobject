using System;

namespace ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEquals = DeepValueEquals.FromProperties<T>();

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (obj is T other &&
            Equals(other));

        public bool Equals(T other) =>
            other != null &&
            (ReferenceEquals(other, this) ||
            DeepEquals(this, other));

        public static implicit operator T(ValueObject<T> valueObject) =>
            (T)valueObject;        
    }
}
