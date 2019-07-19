using System;

namespace ValueObject
{
    public abstract class FieldValueObject<T> : IEquatable<T>
        where T : FieldValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEquals = DeepValueEquals.FromFields<T>();

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (obj is T other &&
            Equals(other));

        public bool Equals(T other) =>
            other != null &&
            (ReferenceEquals(other, this) ||
            DeepEquals(this, other));

        public static implicit operator T(FieldValueObject<T> valueObject) =>
            (T)valueObject;        
    }
}
