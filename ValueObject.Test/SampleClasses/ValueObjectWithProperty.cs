namespace ValueObject.Test
{
    public class ValueObjectWithProperty<T> : ValueObject<ValueObjectWithProperty<T>>
    {
        public ValueObjectWithProperty(T value1)
        {
            Value1 = value1;
        }

        public T Value1 { get; }
    }
}
