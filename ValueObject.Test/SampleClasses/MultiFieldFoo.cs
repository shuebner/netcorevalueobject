using System;

namespace ValueObject.Test
{
    public class MultiFieldFoo : FieldValueObject<MultiFieldFoo>
    {
        public readonly string Value1;
        public readonly string Value2;
        public readonly string Value3;
        public readonly string Value4;
        public readonly string Value5;
        public readonly string Value6;
        public readonly string Value7;

        public MultiFieldFoo(
            string value1,
            string value2,
            string value3,
            string value4,
            string value5,
            string value6,
            string value7)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
            Value6 = value6;
            Value7 = value7;
        }
    }
}
