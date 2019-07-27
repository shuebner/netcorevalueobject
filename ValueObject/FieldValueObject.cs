using System;

namespace ValueObject
{
    public abstract class FieldValueObject<T> : ValueObject<T>
        where T : FieldValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEqualsFunc = DeepValueEquals.FromFields<T>();

        protected override bool DeepEquals(T one, T other) => DeepEqualsFunc(one, other);
    }
}
