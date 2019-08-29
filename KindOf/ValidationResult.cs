namespace KindOf
{
    public class ValidationResult
    {
        public static ValidationResult<T> Success<T>() => new ValidationResult<T>.Success();

        public static ValidationResult<T> Error<T>(string errorMessage, T invalidValue) =>
            new ValidationResult<T>.Error(errorMessage, invalidValue);
    }

    public abstract class ValidationResult<T>
    {
        private ValidationResult()
        {
        }

        public class Success
            : ValidationResult<T>
        {
            internal Success()
            {
            }
        }

        public class Error
            : ValidationResult<T>
        {
            public readonly string ErrorMessage;
            public readonly T OriginalValue;

            internal Error(string errorMessage, T invalidValue)
            {
                ErrorMessage = errorMessage;
                OriginalValue = invalidValue;
            }
        }
    }
}
