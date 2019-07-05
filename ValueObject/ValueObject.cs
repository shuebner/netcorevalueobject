using System;
using System.Linq;
using System.Linq.Expressions;

namespace ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        private static readonly ParameterExpression OneParam = Expression.Parameter(typeof(T), "one");
        private static readonly ParameterExpression OtherParam = Expression.Parameter(typeof(T), "other");

        private static readonly string[] PropertyNames = typeof(T).GetProperties()
                .Select(p => p.Name)
                .ToArray();

        private static readonly Func<T, T, bool> GenericEquals = PropertyNames.Any()
            ? GenerateGenericEquals()
            : (one, other) => true;

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (obj is T other &&
            Equals(other));

        public bool Equals(T other) =>
            other != null &&
            (ReferenceEquals(other, this) ||
            GenericEquals(this, other));

        public static implicit operator T(ValueObject<T> valueObject) =>
            (T)valueObject;

        private static Func<T, T, bool> GenerateGenericEquals()
        {
            Expression andExpression = PropertyNames.Skip(1).Aggregate
                (EqualsExpr(PropertyNames.First()),
                (exp, propName) => Expression.AndAlso(
                        exp,
                        EqualsExpr(propName)));

            var equalsExpression = Expression.Lambda<Func<T, T, bool>>(
                andExpression,
                new[] { OneParam, OtherParam });

            return equalsExpression.Compile();

            BinaryExpression EqualsExpr(string propertyName) =>
                Expression.Equal(
                    Expression.Property(OneParam, propertyName),
                    Expression.Property(OtherParam, propertyName));
        }
    }
}
