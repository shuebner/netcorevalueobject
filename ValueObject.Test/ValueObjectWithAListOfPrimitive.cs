using System.Collections.Immutable;

namespace ValueObject.Test
{
    public class ValueObjectWithAListOfPrimitive : ValueObject<ValueObjectWithAListOfPrimitive>
    {
        public ValueObjectWithAListOfPrimitive(params string[] values)
        {
            Values = values.ToImmutableArray();
        }

        public ImmutableArray<string> Values { get; }
    }
}
