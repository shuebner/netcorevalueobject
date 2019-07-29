using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ValueObject
{
    public abstract class FieldValueObject<T> : ValueObject<T>
        where T : FieldValueObject<T>
    {
        private static readonly Func<T, T, bool> DeepEqualsFunc = DeepValueEquals.FromFields<T>();

        protected override bool DeepEquals(T one, T other) => DeepEqualsFunc(one, other);

        public override int GetHashCode()
        {
            var fields = typeof(T).GetFields();
            var values = fields.Select(field => field.GetValue(this));

            return values.Aggregate(
                0,
                (hash, value) => GetHashCodeInternal(0, hash, value));
        }

        private static int GetHashCodeInternal(int counter, int currentHash, object value)
        {
            counter++;
            currentHash <<= counter;
            if (value is IEnumerable<int> list)
            {
                return list.Aggregate(0, (hash, element) => GetHashCodeInternal(counter, hash, element)) + currentHash;
            }
            else
            {
                return (value?.GetHashCode() ?? 0) * 31 + currentHash;
            }
        }
    }
}
