using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ImmutableObject
{
    public static class DeepValueEquals
    {
        private delegate MemberExpression GetMemberAccessor(Expression param, string name);
        
        private static readonly Type[] ValidSequenceTypes = { typeof(ImmutableArray<>), typeof(ImmutableList<>) };
        private static readonly Type[] ValueTypeEquivalents = { typeof(string) };

        public static Func<T, T, bool> FromFields<T>() =>
            ForMembers<T, FieldInfo>(typeof(T).GetFields(), fieldInfo => fieldInfo.FieldType, Expression.Field);

        public static Func<T, T, bool> FromProperties<T>() =>
            ForMembers<T, PropertyInfo>(typeof(T).GetProperties(), propertyInfo => propertyInfo.PropertyType, Expression.Property);

        private static Func<T, T, bool> ForMembers<T, TMember>(
            IEnumerable<TMember> memberInfos,
            Func<TMember, Type> getMemberType,
            GetMemberAccessor getMemberAccessor)
            where TMember : MemberInfo
        {
            ParameterExpression oneParam = Expression.Parameter(typeof(T), "one");
            ParameterExpression otherParam = Expression.Parameter(typeof(T), "other");

            if (!memberInfos.Any())
            {
                return Expression.Lambda<Func<T, T, bool>>(
                    Expression.Constant(true),
                    oneParam,
                    otherParam)
                    .Compile();
            }

            var equalityExpressions = memberInfos.Select(memberInfo =>
                EqualityExpr(oneParam, otherParam, memberInfo.Name, getMemberType(memberInfo), getMemberAccessor));
            var andExpression = equalityExpressions.Aggregate(Expression.AndAlso);

            Expression<Func<T, T, bool>> equalsExpression = Expression.Lambda<Func<T, T, bool>>(
                andExpression,
                new[] { oneParam, otherParam });

            return equalsExpression.Compile();
        }

        private static Expression EqualityExpr(Expression oneParam, Expression otherParam, string memberName, Type memberType,
            GetMemberAccessor getMemberAccessor)
        {
            if (memberType.IsGenericType && ValidSequenceTypes.Contains(memberType.GetGenericTypeDefinition()))
            {
                return SequenceEqualsExpr(oneParam, otherParam, memberName, memberType.GenericTypeArguments.Single(), getMemberAccessor);
            }

            if (memberType.IsValueType || ValueTypeEquivalents.Contains(memberType))
            {
                return EqualOperatorExpr(oneParam, otherParam, memberName, getMemberAccessor);
            }

            if (memberType.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEquatable<>) &&
                i.GenericTypeArguments.Single() == memberType))
            {
                return
                    Expression.OrElse(
                        EqualOperatorExpr(oneParam, otherParam, memberName, getMemberAccessor),
                        Expression.AndAlso(
                            Expression.Not(
                                Expression.ExclusiveOr(
                                    Expression.Equal(
                                        getMemberAccessor(oneParam, memberName),
                                        Expression.Constant(null)),
                                    Expression.Equal(
                                        getMemberAccessor(otherParam, memberName),
                                        Expression.Constant(null)))),
                            EqualsExpr(oneParam, otherParam, memberName, memberType, getMemberAccessor)));
            }

            throw new ArgumentException($"cannot handle field {memberName} of type {memberType.FullName}");
        }

        private static MethodCallExpression SequenceEqualsExpr(
            Expression oneParam, Expression otherParam, string memberName, Type elementType, GetMemberAccessor getMemberAccessor)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

            return Expression.Call(
                null,
                Methods.SequenceEqual(elementType),
                Expression.Convert(
                    getMemberAccessor(oneParam, memberName), enumerableType),
                Expression.Convert(
                    getMemberAccessor(otherParam, memberName), enumerableType));
        }

        private static MethodCallExpression EqualsExpr(
            Expression oneParam, Expression otherParam, string memberName, Type memberType, GetMemberAccessor getMemberAccessor) =>
            Expression.Call(
                getMemberAccessor(oneParam, memberName),
                Methods.EquatableEquals(memberType),
                getMemberAccessor(otherParam, memberName));

        private static BinaryExpression EqualOperatorExpr(
            Expression oneParam, Expression otherParam, string fieldName, GetMemberAccessor getMemberAccessor) =>
            Expression.Equal(
                getMemberAccessor(oneParam, fieldName),
                getMemberAccessor(otherParam, fieldName));

        
    }
}
