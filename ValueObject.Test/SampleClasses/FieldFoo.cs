using System;

namespace ValueObject.Test
{
    public class FieldFoo<T> : FieldValueObject<FieldFoo<T>>
    {
        public readonly T Value;

        public FieldFoo(T value)
        {
            Value = value;
        }
    }
}
