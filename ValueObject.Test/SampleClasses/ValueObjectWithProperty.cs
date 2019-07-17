namespace ValueObject.Test
{
    public class ValueObjectWithProperty<T> : ValueObject<ValueObjectWithProperty<T>>
    {
        public ValueObjectWithProperty(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
