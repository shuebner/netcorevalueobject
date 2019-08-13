using System;

namespace KindOf
{
    public abstract class OfString : Of<string>
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
    }
}
