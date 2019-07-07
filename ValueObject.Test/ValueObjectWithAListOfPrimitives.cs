using System.Collections.Immutable;

namespace ValueObject.Test
{
    public class ValueObjectWithAListOfPrimitives<T> : ValueObject<ValueObjectWithAListOfPrimitives<T>>
    {
        public ValueObjectWithAListOfPrimitives(params T[] values)
        {
            Values = values.ToImmutableList();
        }

        public ImmutableList<T> Values { get; }
    }
}
