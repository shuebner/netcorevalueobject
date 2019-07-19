namespace ValueObject.Test
{
    public class PropertyFoo<T> : PropertyValueObject<PropertyFoo<T>>
    {
        public PropertyFoo(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
