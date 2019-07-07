namespace ValueObject.Test
{
    public class ValueObjectWithValue<T> : ValueObject<ValueObjectWithValue<T>>
    {
        public ValueObjectWithValue(T value1)
        {
            Value1 = value1;
        }

        public T Value1 { get; }
    }
}
