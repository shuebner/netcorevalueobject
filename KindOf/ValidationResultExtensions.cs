using System;

namespace KindOf
{
    public static class ValidationResultExtensions
    {
        public static T Match<TValue, T>(this ValidationResult<TValue> result, Func<T> onSuccess, Func<ValidationResult<TValue>.Error, T> onError) =>
            result is ValidationResult<TValue>.Error error
                ? onError(error)
                : onSuccess();
    }
}
