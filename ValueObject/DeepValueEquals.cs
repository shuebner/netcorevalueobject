using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ValueObject
{
    public static class DeepValueEquals
    {
        private static readonly Type[] ValidSequenceTypes = { typeof(ImmutableArray<>), typeof(ImmutableList<>) };
        private static readonly Type[] ValueTypeEquivalents = { typeof(string) };

        public static Func<T, T, bool> FromFields<T>()
        {
            ParameterExpression OneParam = Expression.Parameter(typeof(T), "one");
            ParameterExpression OtherParam = Expression.Parameter(typeof(T), "other");

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            if (!fieldInfos.Any())
            {
                return Expression.Lambda<Func<T, T, bool>>(
                    Expression.Constant(true),
                    OneParam,
                    OtherParam)
                    .Compile();
            }

            var equalityExpressions = fieldInfos.Select(EqualityExpr);
            var andExpression = equalityExpressions.Aggregate(Expression.AndAlso);

            Expression<Func<T, T, bool>> equalsExpression = Expression.Lambda<Func<T, T, bool>>(
                andExpression,
                new[] { OneParam, OtherParam });

            return equalsExpression.Compile();

            Expression EqualityExpr(FieldInfo fieldInfo)
            {
                var fieldName = fieldInfo.Name;
                var fieldType = fieldInfo.FieldType;

                if (fieldType.IsGenericType && ValidSequenceTypes.Contains(fieldType.GetGenericTypeDefinition()))
                {
                    return SequenceEqualsExpr(fieldName, fieldType.GenericTypeArguments.Single());
                }

                if (fieldType.IsValueType || ValueTypeEquivalents.Contains(fieldType))
                {
                    return EqualOperatorExpr(fieldName);
                }

                if (fieldType.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEquatable<>) &&
                    i.GenericTypeArguments.Single() == fieldType))
                {
                    return
                        Expression.OrElse(
                            EqualOperatorExpr(fieldName),
                            Expression.AndAlso(
                                Expression.Not(
                                    Expression.ExclusiveOr(
                                        Expression.Equal(
                                            Expression.Field(OneParam, fieldName),
                                            Expression.Constant(null)),
                                        Expression.Equal(
                                            Expression.Field(OtherParam, fieldName),
                                            Expression.Constant(null)))),
                                EqualsExpr(fieldName, fieldType)));
                }

                throw new ArgumentException($"cannot handle field {fieldName} of type {fieldType.FullName}");
            }

            MethodCallExpression SequenceEqualsExpr(string fieldName, Type elementType)
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

                return Expression.Call(
                    null,
                    Methods.SequenceEqual(elementType),
                    Expression.Convert(
                        Expression.Field(OneParam, fieldName), enumerableType),
                    Expression.Convert(
                        Expression.Field(OtherParam, fieldName), enumerableType));
            }

            MethodCallExpression EqualsExpr(string fieldName, Type fieldType) =>
                Expression.Call(
                    Expression.Field(OneParam, fieldName),
                    Methods.EquatableEquals(fieldType),
                    Expression.Field(OtherParam, fieldName));

            BinaryExpression EqualOperatorExpr(string fieldName) =>
                Expression.Equal(
                    Expression.Field(OneParam, fieldName),
                    Expression.Field(OtherParam, fieldName));
        }

        public static Func<T, T, bool> FromProperties<T>()
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

            var equalityExpressions = propertyInfos.Select(EqualityExpr);
            var andExpression = equalityExpressions.Aggregate(Expression.AndAlso);

            Expression<Func<T, T, bool>> equalsExpression = Expression.Lambda<Func<T, T, bool>>(
                andExpression,
                new[] { OneParam, OtherParam });

            return equalsExpression.Compile();

            Expression EqualityExpr(PropertyInfo propertyInfo)
            {
                var propertyName = propertyInfo.Name;
                var propertyType = propertyInfo.PropertyType;
                
                if (propertyType.IsGenericType && ValidSequenceTypes.Contains(propertyType.GetGenericTypeDefinition()))
                {
                    return SequenceEqualsExpr(propertyName, propertyType.GenericTypeArguments.Single());
                }

                if (propertyType.IsValueType || ValueTypeEquivalents.Contains(propertyType))
                {
                    return EqualOperatorExpr(propertyName);
                }

                if (propertyType.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEquatable<>) &&
                    i.GenericTypeArguments.Single() == propertyType))
                {
                    return
                        Expression.OrElse(
                            EqualOperatorExpr(propertyName),
                            Expression.AndAlso(
                                Expression.Not(
                                    Expression.ExclusiveOr(
                                        Expression.Equal(
                                            Expression.Property(OneParam, propertyName),
                                            Expression.Constant(null)),
                                        Expression.Equal(
                                            Expression.Property(OtherParam, propertyName),
                                            Expression.Constant(null)))),
                                EqualsExpr(propertyName, propertyType)));
                }

                throw new ArgumentException($"cannot handle property {propertyName} of type {propertyType.FullName}");
            }

            MethodCallExpression SequenceEqualsExpr(string propertyName, Type elementType)
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

                return Expression.Call(
                    null,
                    Methods.SequenceEqual(elementType),
                    Expression.Convert(
                        Expression.Property(OneParam, propertyName), enumerableType),
                    Expression.Convert(
                        Expression.Property(OtherParam, propertyName), enumerableType));
            }

            MethodCallExpression EqualsExpr(string propertyName, Type propertyType) =>
                Expression.Call(
                    Expression.Property(OneParam, propertyName),
                    Methods.EquatableEquals(propertyType),
                    Expression.Property(OtherParam, propertyName));

            BinaryExpression EqualOperatorExpr(string propertyName) =>
                Expression.Equal(
                    Expression.Property(OneParam, propertyName),
                    Expression.Property(OtherParam, propertyName));
        }
    }
}
