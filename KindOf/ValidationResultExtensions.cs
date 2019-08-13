using System;

namespace KindOf
{
    public static class ValidationResultExtensions
    {
        public static T Match<TValue, T>(this ValidationResult<TValue> result, Func<T> onSuccess, Func<ValidationResult<TValue>.ErrorResult, T> onError) =>
            result is ValidationResult<TValue>.ErrorResult error
                ? onError(error)
                : onSuccess();
    }
}
