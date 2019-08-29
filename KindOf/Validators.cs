using System;
using System.Linq;

namespace KindOf
{
    public static class Validators
    {
        public static Func<T, ValidationResult<T>> AllOf<T>(params Func<T, ValidationResult<T>>[] validators) =>
            str => validators
                .Select(validate => validate(str))
                .FirstOrDefault(result => result is ValidationResult<T>.Error) is ValidationResult<T>.Error error
                    ? error
                    : ValidationResult.Success<T>();

        public static Func<T, ValidationResult<T>> None<T>() => str => ValidationResult.Success<T>();
    }
}
