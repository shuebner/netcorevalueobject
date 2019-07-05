using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ValueObject.Test
{
    public class ValueObjectTests
    {
        private readonly ITestOutputHelper output;

        public ValueObjectTests(ITestOutputHelper output)
        {
            this.output = output;
        }

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

        [Fact]
        public void Equals_is_an_order_of_magnitude_faster_reflection()
        {
            var foo = new ValueObjectWithPrimitives("foo");
            var bar = new ValueObjectWithPrimitives("bar");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 1_000_000; i++)
            {
                ReflectionEquals(foo, bar);
            }
            var reflectionEqualsMilliSeconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Start();

            for (var i = 0; i < 1_000_000; i++)
            {
                foo.Equals(bar);
            }

            var equalsMilliseconds = stopwatch.ElapsedMilliseconds;
            equalsMilliseconds.Should().BeLessThan(
                reflectionEqualsMilliSeconds / 10);
        }

        private static bool ReflectionEquals<T>(T one, T other)
        {
            var properties = typeof(T).GetProperties();

            return properties.All(prop =>
                Equals(prop.GetValue(one), prop.GetValue(other)));
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
