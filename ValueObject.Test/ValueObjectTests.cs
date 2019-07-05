using FluentAssertions;
using System;
using Xunit;

namespace ValueObject.Test
{
    public class ValueObjectTests
    {
        [Fact]
        public void ObjectEquals_When_other_is_null_Then_returns_false()
        {
            var isEqual = ((object)new EmptyValueObject()).Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_of_other_type_Then_returns_false()
        {
            var isEqual = ((object)new EmptyValueObject()).Equals("42");

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new EmptyValueObject();
            var isEqual = ((object)valueObject).Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_other_is_null_Then_returns_false()
        {
            var isEqual = new EmptyValueObject().Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new EmptyValueObject();
            var isEqual = valueObject.Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_value_is_different_Then_returns_false()
        {
            var isEqual = new ValueObjectWithPrimitives("foo")
                .Equals(new ValueObjectWithPrimitives("bar"));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_primitive_value_is_equal_Then_returns_true()
        {
            var isEqual = new ValueObjectWithPrimitives("foo")
                .Equals(new ValueObjectWithPrimitives("foo"));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_values_are_different_Then_returns_false()
        {
            var isEqual = new ValueObjectWithPrimitives("foo").Equals(new ValueObjectWithPrimitives("bar"));

            isEqual.Should().BeFalse();
        }
    }

    public class EmptyValueObject : ValueObject<EmptyValueObject>
    {
    }

    public class ValueObjectWithPrimitives : ValueObject<ValueObjectWithPrimitives>
    {
        public ValueObjectWithPrimitives(string value)
        {
            Value = value ?? throw new ArgumentNullException(value);
        }

        public string Value { get; }
    }
}
