using System;

namespace ValueObject.Test
{
    public class ValueObjectWithAPrimitive : ValueObject<ValueObjectWithAPrimitive>
    {
        public ValueObjectWithAPrimitive(string value1)
        {
            Value1 = value1 ?? throw new ArgumentNullException(nameof(value1));
        }

        public string Value1 { get; }
    }
}
