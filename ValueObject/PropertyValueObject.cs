using ImmutableObject;
using System;

namespace ValueObject
{
    public abstract class PropertyValueObject<T> : ValueObject<T>
        where T : PropertyValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEqualsFunc = DeepValueEquals.FromProperties<T>();

        protected override bool DeepEquals(T one, T other) => DeepEqualsFunc(one, other);
    }
}
