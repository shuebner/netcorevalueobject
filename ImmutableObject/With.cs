using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ImmutableObject
{
    public static class With
    {
        public static Func<T, string, object, T> For<T>()
        {
            var type = typeof(T);
            ConstructorInfo ctor = type.GetConstructors().Single();
            ParameterInfo[] ctorParamInfos = ctor.GetParameters();
            ImmutableDictionary<string, Func<T, object, T>> createNewObjectByFieldName = type
                .GetFields()
                .ToImmutableDictionary(field => field.Name, field => GetConstructorWithFieldValue<T>(ctor, ctorParamInfos, field.Name));

            ParameterExpression obj = Expression.Parameter(typeof(T), "obj");
            ParameterExpression memberName = Expression.Parameter(typeof(string), "memberName");
            ParameterExpression newValue = Expression.Parameter(typeof(object), "newValue");
            
            var getCtorFuncExpression =
                Expression.Property(
                    Expression.Constant(createNewObjectByFieldName),
                    "Item",
                    memberName);

            var getNewObjWithNewValueExpression =
                Expression.Invoke(
                    getCtorFuncExpression,
                    obj,
                    newValue);

            var with = Expression.Lambda<Func<T, string, object, T>>(
                getNewObjWithNewValueExpression,
                obj,
                memberName,
                newValue);

            return with.Compile();
        }

        private static Func<T, object, T> GetConstructorWithFieldValue<T>(ConstructorInfo ctor, ParameterInfo[] ctorParamInfos, string fieldName)
        {
            var originalObjExpr = Expression.Parameter(typeof(T), "original");
            var field = typeof(T).GetField(fieldName);
            var newValueExpr = Expression.Parameter(typeof(object), "value");

            var parameterName = ToParameterName(fieldName);
            var ctorExpression = Expression.New(ctor,
                ctorParamInfos.Select(param => param.Name != parameterName
                    ? (Expression)Expression.Field(originalObjExpr, typeof(T).GetField(ToMemberName(param.Name)))
                    : Expression.Convert(newValueExpr, field.FieldType)));

            return Expression.Lambda<Func<T, object, T>>(ctorExpression, originalObjExpr, newValueExpr).Compile();
        }

        private static string ToParameterName(string memberName) =>
            string.Concat(char.ToLowerInvariant(memberName[0]), memberName.Substring(1));

        private static string ToMemberName(string parameterName) =>
            string.Concat(char.ToUpperInvariant(parameterName[0]), parameterName.Substring(1));
    }
}
