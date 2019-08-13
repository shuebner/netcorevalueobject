using KindOf;
using System;
using System.Globalization;
using System.Linq.Expressions;
using ValueObject;

namespace Example
{
    public class Person
    {
        private static readonly Func<Person, string, object, Person> WithFunc = ImmutableObject.With.For<Person>();

        public readonly Name Name;
        public readonly ContactInfo ContactInfo;

        public Person(Name name, ContactInfo contactInfo)
        {
            Name = name;
            ContactInfo = contactInfo;
        }

        public Person With<T>(Expression<Func<Person, T>> propertyExpression, T value) => 
            WithFunc(this, ((MemberExpression)propertyExpression.Body).Member.Name, value);
    }

    public class Name : FieldValueObject<Name>
    {
        public readonly FirstName FirstName;
        public readonly LastName LastName;

        public Name(FirstName firstName, LastName lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class ContactInfo : FieldValueObject<ContactInfo>
    {
        public readonly EmailAddress EmailAddress;

        public ContactInfo(EmailAddress emailAddress)
        {
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

    public class PersonService
    {
        public static Person MarryTo(Person person, Person spouse)
        {
            var newName = person.Name.With(name => name.LastName, spouse.Name.LastName);
            return person.With(p => p.Name, newName);
        }
    }
}
