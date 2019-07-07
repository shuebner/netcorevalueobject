using System.Collections.Immutable;

namespace ValueObject.Test
{
    public class ValueObjectWithAnArrayOfPrimitives<T> : ValueObject<ValueObjectWithAnArrayOfPrimitives<T>>
    {
        public ValueObjectWithAnArrayOfPrimitives(params T[] values)
        {
            Values = values.ToImmutableArray();
        }

        public ImmutableArray<T> Values { get; }
    }
}
