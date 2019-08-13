using KindOf;
using System.Globalization;
using ValueObject;

namespace Example
{
    public class Person : FieldValueObject<Person>
    {
        public readonly FirstName FirstName;
        public readonly LastName LastName;
        public readonly EmailAddress EmailAddress;

        public Person(FirstName firstName, LastName lastName, EmailAddress emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
    }

    public class FirstName : OfString
    {
        public FirstName(string value)
            : base(value,
                  Validators.AllOf(
                      StringValidators.MinLength(2),
                      StringValidators.MaxLength(50),
                      StringValidators.SingleLine,
                      StringValidators.NoWhitespace),
                  Canonicalizers.None<string>())
        {
        }
    }

    public class LastName : OfString
    {
        public LastName(string value)
            : base(value,
                  Validators.AllOf(
                      StringValidators.MinLength(2),
                      StringValidators.MaxLength(50),
                      StringValidators.SingleLine,
                      StringValidators.NoWhitespace),
                  Canonicalizers.None<string>())
        {
        }
    }

    public class EmailAddress : OfString
    {
        public EmailAddress(string value)
            : base(value,
                  Validators.AllOf(
                      StringValidators.SingleLine,
                      StringValidators.NoWhitespace),
                  StringCanonicalizers.LowerCase(CultureInfo.InvariantCulture))
        {
        }
    }
}
