using ImmutableObject;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ValueObject
{
    public abstract class FieldValueObject<T> : ValueObject<T>
        where T : FieldValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEqualsFunc = DeepValueEquals.FromFields<T>();
        private static readonly Func<T, LambdaExpression, object, T> WithFunc = ImmutableObject.With.For<T>();

        protected override bool DeepEquals(T one, T other) => DeepEqualsFunc(one, other);

        public T With<TValue>(Expression<Func<T, TValue>> fieldExpression, TValue value) =>
            WithFunc(thisAsT, fieldExpression, value);
    }
}
