using System;

namespace KindOf
{
    public abstract class OfString<T> : Of<string>, IComparable<T>
        where T : OfString<T>
    {
        public OfString(string value, Func<string, ValidationResult<string>> validate, Func<string, string> canonicalize)
            : base(
                  validate(value).Match(
                      () => canonicalize(value),
                      error => throw new ArgumentException(
                          string.Join("\n", error.ErrorMessage, $"original value: {error.OriginalValue}"),
                          nameof(value))))
        {
        }

        public int CompareTo(T other) => Value.CompareTo(other.Value);

        public override string ToString() => Value;
    }
}
