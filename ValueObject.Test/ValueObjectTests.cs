using FluentAssertions;
using Xunit;

namespace ValueObject.Test
{
    public class ValueObjectTests
    {
        [Fact]
        public void ObjectEquals_When_other_is_null_Then_returns_false()
        {
            var isEqual = ((object)new ValueObjectWithAPrimitive("42")).Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_of_other_type_Then_returns_false()
        {
            var isEqual = ((object)new ValueObjectWithAPrimitive("42")).Equals("42");

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new ValueObjectWithAPrimitive("42");
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
            var isEqual = new ValueObjectWithAPrimitive("42").Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new ValueObjectWithAPrimitive("42");
            var isEqual = valueObject.Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_value_is_different_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAPrimitive("foo")
                .Equals(new ValueObjectWithAPrimitive("bar"));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_primitive_value_is_equal_Then_returns_true()
        {
            var isEqual = new ValueObjectWithAPrimitive("foo")
                .Equals(new ValueObjectWithAPrimitive("foo"));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_values_are_different_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAPrimitive("foo")
                .Equals(new ValueObjectWithAPrimitive("bar"));

            isEqual.Should().BeFalse();
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
                .Equals(new ValueObjectWithAnArrayOfPrimitives<int>(1, 2, 3, 4));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_same_order_Then_returns_true()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<string>("one", "two", "three")
                .Equals(new ValueObjectWithAListOfPrimitives<string>("one", "two", "three"));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_different_order_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<string>("one", "two", "three")
                .Equals(new ValueObjectWithAListOfPrimitives<string>("one", "three", "two"));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_different_primitives_Then_returns_false()
        {
            var isEqual = new ValueObjectWithAListOfPrimitives<string>("one", "two", "three")
                .Equals(new ValueObjectWithAListOfPrimitives<string>("one", "two", "three", "four"));

            isEqual.Should().BeFalse();
        }
    }
}
