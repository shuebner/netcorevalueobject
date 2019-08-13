using FluentAssertions;
using System;
using Xunit;

namespace KindOf
{
    public class ValidatorsTests
    {
        [Fact]
        public void AllOf_When_any_validator_fails_Then_fails_with_first_error_result()
        {
            Func<string, ValidationResult<string>> successful = _ => ValidationResult.Success<string>();
            Func<string, ValidationResult<string>> firstError = str => ValidationResult.Error("first", str);
            Func<string, ValidationResult<string>> secondError = str => ValidationResult.Error("second", str);

            var result = Validators.AllOf(successful, firstError, secondError)("foo");

            result.Should().BeAssignableTo<ValidationResult<string>.ErrorResult>().Which.ErrorMessage.Should().Be("first");
        }
    }
}
