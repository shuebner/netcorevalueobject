using System.Collections.Immutable;

namespace ValueObject.Test
{
    public class ValueObjectWithAnArrayOfPrimitives : ValueObject<ValueObjectWithAnArrayOfPrimitives>
    {
        public ValueObjectWithAnArrayOfPrimitives(params string[] values)
        {
            Values = values.ToImmutableArray();
        }

        public ImmutableArray<string> Values { get; }
    }
}
