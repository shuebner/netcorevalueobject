﻿using System;
using System.Linq;
using StringValidator = System.Func<string, KindOf.ValidationResult<string>>;

namespace KindOf
{
    public static class StringValidators
    {
        public static StringValidator NonEmpty => From(str => str == string.Empty, "must be non-empty");

        public static StringValidator SingleLine => From(str => str.Contains("\n") || str.Contains("\r"), "must be single line");

        public static StringValidator NoWhitespace => From(str => !str.Any(c => char.IsWhiteSpace(c)), "cannot contain whitespace");

        public static StringValidator MaxLength(int maxLength) =>
            From(str => str.Length > maxLength, $"cannot be longer than {maxLength} characters");

        public static StringValidator MinLength(int minLength) =>
            From(str => str.Length < minLength, $"cannot be shorter than {minLength} characters");

        public static StringValidator From(Func<string, bool> isValid, string invalidMessage) =>
            str => isValid(str)
                ? ValidationResult.Success<string>()
                : ValidationResult.Error(invalidMessage, str);
    }
}
