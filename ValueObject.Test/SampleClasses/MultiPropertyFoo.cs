using System;

namespace ValueObject.Test
{
    public class MultiPropertyFoo : PropertyValueObject<MultiPropertyFoo>
    {
        public MultiPropertyFoo(
            string value1,
            string value2,
            string value3,
            string value4,
            string value5,
            string value6,
            string value7)
        {
            Value1 = value1 ?? throw new ArgumentNullException(nameof(value1));
            Value2 = value2 ?? throw new ArgumentNullException(nameof(value2));
            Value3 = value3 ?? throw new ArgumentNullException(nameof(value3));
            Value4 = value4 ?? throw new ArgumentNullException(nameof(value4));
            Value5 = value5 ?? throw new ArgumentNullException(nameof(value5));
            Value6 = value6 ?? throw new ArgumentNullException(nameof(value6));
            Value7 = value7 ?? throw new ArgumentNullException(nameof(value7));
        }

        public string Value1 { get; }
        public string Value2 { get; }
        public string Value3 { get; }
        public string Value4 { get; }
        public string Value5 { get; }
        public string Value6 { get; }
        public string Value7 { get; }
    }
}
