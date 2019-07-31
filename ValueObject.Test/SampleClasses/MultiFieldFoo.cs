using System;

namespace ValueObject.Test
{
    public class MultiFieldFoo : FieldValueObject<MultiFieldFoo>
    {
        public MultiFieldFoo(
            string value1,
            string value2,
            string value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public readonly string Value1;
        public readonly string Value2;
        public readonly string Value3;
    }
}
