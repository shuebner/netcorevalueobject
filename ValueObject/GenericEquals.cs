using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ValueObject
{
    public static class GenericEquals
    {
        public static Func<T, T, bool> For<T>()
        {
            ParameterExpression OneParam = Expression.Parameter(typeof(T), "one");
            ParameterExpression OtherParam = Expression.Parameter(typeof(T), "other");

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            if (!propertyInfos.Any())
            {
                return Expression.Lambda<Func<T, T, bool>>(
                    Expression.Constant(true),
                    OneParam,
                    OtherParam)
                    .Compile();
            }

            string[] PropertyNames = propertyInfos
                .Select(p => p.Name)
                .ToArray();

            Expression andExpression = propertyInfos.Skip(1).Aggregate
                (EqualityExpr(propertyInfos.First()),
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

                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ImmutableArray<>))
                {
                    return SequenceEqualsExpr(propertyName, typeof(string));
                }
                return EqualsExpr(propertyInfo.Name);
            }

            MethodCallExpression SequenceEqualsExpr(string propertyName, Type elementType)
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

                return Expression.Call(null,
                    Methods.SequenceEqual(elementType),
                    Expression.Convert(
                        Expression.Property(OneParam, propertyName), enumerableType),
                    Expression.Convert(
                        Expression.Property(OtherParam, propertyName), enumerableType));
            }

            BinaryExpression EqualsExpr(string propertyName) =>
                Expression.Equal(
                    Expression.Property(OneParam, propertyName),
                    Expression.Property(OtherParam, propertyName));
        }
    }
}
