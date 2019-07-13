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
            var isEqual = ((object)new ValueObjectWithValue<string>("42")).Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_of_other_type_Then_returns_false()
        {
            var isEqual = ((object)new ValueObjectWithValue<string>("42")).Equals("42");

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new ValueObjectWithValue<string>("42");
            var isEqual = ((object)valueObject).Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_valueobject_has_no_properties_and_other_is_different_reference_Then_returns_true()
        {
            var isEqual = new EmptyValueObject().Equals(new EmptyValueObject());

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_other_is_null_Then_returns_false()
        {
            var isEqual = new ValueObjectWithValue<string>("42").Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new ValueObjectWithValue<string>("42");
            var isEqual = valueObject.Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_value_is_different_Then_returns_false()
        {
            var isEqual = new ValueObjectWithValue<int>(1)
                .Equals(new ValueObjectWithValue<int>(2));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_primitive_value_is_equal_Then_returns_true()
        {
            var isEqual = new ValueObjectWithValue<int>(1)
                .Equals(new ValueObjectWithValue<int>(1));

            isEqual.Should().BeTrue();
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData(null, "foo")]
        [InlineData("foo", null)]
        public void Equals_When_string_value_is_different_Then_returns_false(string value1, string value2)
        {
            var isEqual = new ValueObjectWithValue<string>(value1)
                .Equals(new ValueObjectWithValue<string>(value2));

            isEqual.Should().BeFalse();
        }

        [Theory]
        [InlineData("foo")]
        [InlineData(null)]
        public void Equals_When_string_value_is_equal_Then_returns_true(string value)
        {
            var isEqual = new ValueObjectWithValue<string>(value)
                .Equals(new ValueObjectWithValue<string>(value));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_equatables_are_equal_Then_returns_true()
        {
            var isEqual = new ValueObjectWithValue<SomeEquatable>(new SomeEquatable("42"))
                .Equals(new ValueObjectWithValue<SomeEquatable>(new SomeEquatable("42")));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_equatables_are_not_equal_Then_returns_false()
        {
            var isEqual = new ValueObjectWithValue<SomeEquatable>(new SomeEquatable("42"))
                .Equals(new ValueObjectWithValue<SomeEquatable>(new SomeEquatable("23")));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_equatables_are_both_null_Then_returns_true()
        {
            var isEqual = new ValueObjectWithValue<SomeEquatable>(null)
                .Equals(new ValueObjectWithValue<SomeEquatable>(null));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_array_contains_same_primitives_in_same_order_Then_returns_true()
        {
            var isEqual = new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 3));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_array_contains_same_primitives_in_different_order_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAnArrayOfPrimitives<int>(1, 3, 2));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_array_contains_different_primitives_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 4));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_same_order_Then_returns_true()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAListOfPrimitives<int>(1, 2, 3));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_different_order_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAListOfPrimitives<int>(1, 3, 2));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_different_primitives_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<int>(1, 2, 3)
                .Equals(new ValueObjectWithAListOfPrimitives<int>(1, 2, 4));

            isEqual.Should().BeFalse();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(23, 23)]
        public void Equals_When_nullable_values_are_equal_Then_returns_true(int? oneValue, int? otherValue)
        {
            var isEqual = new ValueObjectWithValue<int?>(oneValue)
                .Equals(new ValueObjectWithValue<int?>(otherValue));

            isEqual.Should().BeTrue();
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(1, null)]
        [InlineData(1, 2)]
        public void Equals_When_nullable_values_are_not_equal_Then_returns_false(int? oneValue, int? otherValue)
        {
            var isEqual = new ValueObjectWithValue<int?>(oneValue)
                .Equals(new ValueObjectWithValue<int?>(otherValue));

            isEqual.Should().BeFalse();
        }

        private class SomeEquatable : IEquatable<SomeEquatable>
        {
            public SomeEquatable(string value)
            {
                Value = value;
            }

            public string Value { get; }

            public bool Equals(SomeEquatable other) =>
                other != null &&
                other.Value == Value;
        }
    }
}
