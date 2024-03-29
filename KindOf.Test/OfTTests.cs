using FluentAssertions;
using System;
using Xunit;

namespace KindOf
{
    public class OfTTests
    {
        [Fact]
        public void Can_be_used_as_underlying_value()
        {
            FirstName firstName = new FirstName("John");

            string firstNameStr = firstName;

            firstNameStr.Should().Be("John");
        }

        [Fact]
        public void Can_not_be_used_as_different_type_with_same_underlying_type()
        {
            FirstName firstName = new FirstName("John");

            firstName.Should().NotBeAssignableTo<LastName>();
        }

        [Fact]
        public void Ctor_When_underlying_value_is_null_Then_throws_ArgumentNullException()
        {
            Action act = () => new FirstName(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Equals_When_types_are_same_and_underlying_value_is_same_Then_returns_true()
        {
            FirstName john1 = new FirstName("John");
            FirstName john2 = new FirstName("John");

            var isEqual = john1.Equals(john2);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_types_are_same_and_underlying_values_are_different_Then_returns_false()
        {
            FirstName john = new FirstName("John");
            FirstName jane = new FirstName("Jane");

            var isEqual = john.Equals(jane);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_types_are_different_and_underlying_value_is_same_Then_returns_false()
        {
            FirstName john1 = new FirstName("John");
            LastName john2 = new LastName("John");

            var isEqual = john1.Equals(john2);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_When_underlying_value_is_different_Then_is_likely_different()
        {
            var john = new FirstName("John");
            var jane = new FirstName("Jane");

            var johnHash = john.GetHashCode();
            var janeHash = jane.GetHashCode();

            johnHash.Should().NotBe(janeHash);
        }

        [Fact]
        public void GetHashCode_When_type_and_underlying_value_is_same_Then_is_same()
        {
            var john1 = new FirstName("John");
            var john2 = new FirstName("John");

            var john1Hash = john1.GetHashCode();
            var john2Hash = john2.GetHashCode();

            john1Hash.Should().Be(john2Hash);
        }

        [Fact]
        public void GetHashCode_When_types_are_different_And_underlying_value_is_same_Then_is_likely_different()
        {
            FirstName john1 = new FirstName("John");
            LastName john2 = new LastName("John");

            var john1Hash = john1.GetHashCode();
            var john2Hash = john2.GetHashCode();

            john1Hash.Should().NotBe(john2Hash);
        }

        private class FirstName : Of<string>
        {
            public FirstName(string value)
                : base(value)
            {
            }
        }

        private class LastName : Of<string>
        {
            public LastName(string value)
                : base(value)
            {
            }
        }
    }
}
