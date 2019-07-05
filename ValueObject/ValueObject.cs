using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        private static readonly MethodInfo SequenceEqualMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(method => method.Name == nameof(Enumerable.SequenceEqual))
            .Where(method => method.GetParameters() is ParameterInfo[] parameters &&
                parameters.Count() == 2 &&
                parameters[0].ParameterType.IsGenericType &&
                parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                parameters[1].ParameterType.IsGenericType &&
                parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Single();

        private static readonly ParameterExpression OneParam = Expression.Parameter(typeof(T), "one");
        private static readonly ParameterExpression OtherParam = Expression.Parameter(typeof(T), "other");

        private static readonly PropertyInfo[] PropertyInfos = typeof(T).GetProperties();
        private static readonly string[] PropertyNames = PropertyInfos
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
            Expression andExpression = PropertyInfos.Skip(1).Aggregate
                (EqualityExpr(PropertyInfos.First()),
                (exp, prop) => Expression.AndAlso(
                        exp,
                        EqualityExpr(prop)));

            Expression<Func<T, T, bool>> equalsExpression = Expression.Lambda<Func<T, T, bool>>(
                andExpression,
                new[] { OneParam, OtherParam });

            return equalsExpression.Compile();

            Expression EqualityExpr(PropertyInfo propertyInfo)
            {
                var propertyName = propertyInfo.Name;

                if (propertyInfo.PropertyType == typeof(ImmutableArray<string>))
                {
                    return Expression.Call(null,
                        SequenceEqualMethod.MakeGenericMethod(typeof(string)),
                        Expression.Convert(
                            Expression.Property(OneParam, propertyName), typeof(IEnumerable<string>)),
                        Expression.Convert(
                            Expression.Property(OtherParam, propertyName), typeof(IEnumerable<string>)));
                }
                return EqualsExpr(propertyInfo.Name);
            }

            BinaryExpression EqualsExpr(string propertyName) =>
                Expression.Equal(
                    Expression.Property(OneParam, propertyName),
                    Expression.Property(OtherParam, propertyName));
        }
    }
}
