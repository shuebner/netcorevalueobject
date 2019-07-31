using System;
using System.Linq;
using System.Linq.Expressions;

namespace ValueObject
{
    public abstract class FieldValueObject<T> : ValueObject<T>
        where T : FieldValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEqualsFunc = DeepValueEquals.FromFields<T>();

        protected override bool DeepEquals(T one, T other) => DeepEqualsFunc(one, other);

        public T With<TValue>(Expression<Func<T, TValue>> fieldExpression, TValue value)
        {
            string propertyName = ((MemberExpression)fieldExpression.Body).Member.Name;
            var ctorParameterName = ToParameterName(propertyName);
            var constructor = typeof(T).GetConstructors().Single();
            var constructorParameters = constructor.GetParameters();
            var properties = typeof(T).GetProperties();
            var ctorParamValues = constructorParameters.Select(param => param.Name == ctorParameterName 
                ? value
                : typeof(T).GetField(ToMemberName(param.Name)).GetValue(this)).ToArray();

            return (T)constructor.Invoke(ctorParamValues);
        }

        private static string ToParameterName(string memberName) =>
            string.Concat(char.ToLowerInvariant(memberName[0]), memberName.Substring(1));

        private static string ToMemberName(string parameterName) =>
            string.Concat(char.ToUpperInvariant(parameterName[0]), parameterName.Substring(1));
    }
}
