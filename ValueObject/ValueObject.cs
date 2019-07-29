using System;

namespace ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
    {
        private readonly T thisAsT;

        protected ValueObject()
        {
            if (this is T foo)
            {
                thisAsT = foo;
            }
            else
            {
                throw new ArgumentException($"type parameter must be the defined value type");
            }
        }

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (obj is T other &&
            Equals(other));

        public bool Equals(T other) =>
            other != null &&
            (ReferenceEquals(other, this) ||
            DeepEquals(thisAsT, other));

        protected abstract bool DeepEquals(T one, T other);

        public override abstract int GetHashCode();
    }
}
