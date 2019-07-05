using System;
using System.Linq;

namespace ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        private static readonly Func<T, T, bool> GenericEquals = GenerateGenericEquals();

        private static readonly string[] PropertyNames = typeof(T).GetProperties()
                .Select(p => p.Name)
                .ToArray();

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (obj is T other &&
            Equals(other));

        public bool Equals(T other) =>
            ReferenceEquals(other, this) ||
            (other is ValueObject<T> otherValueObject &&
            GenericEquals(this, otherValueObject));

        public static implicit operator T(ValueObject<T> valueObject) =>
            (T)valueObject;

        private static Func<T, T, bool> GenerateGenericEquals()
        {
            var properties = typeof(T).GetProperties();

            return (one, other) => properties.All(prop =>
                Equals(prop.GetValue(one), prop.GetValue(other)));
        }
    }
}
