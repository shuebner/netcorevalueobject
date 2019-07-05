using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        [Trait("Category", "Performance")]
        public void Equals_is_an_order_of_magnitude_faster_reflection()
        {
            var foo = new ValueObjectWithManyPrimitives(
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo");
            var bar = new ValueObjectWithManyPrimitives(
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "bar");

            // init
            ReflectionEqualsHelper<ValueObjectWithManyPrimitives>
                .ReflectionEquals(foo, bar);
            const long interations = 1_000_000;

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < interations; i++)
            {
                ReflectionEqualsHelper<ValueObjectWithManyPrimitives>
                    .ReflectionEquals(foo, bar);
            }

            var reflectionEqualsMilliSeconds = stopwatch.ElapsedMilliseconds;
            output.WriteLine($"reflection: {reflectionEqualsMilliSeconds} ms");

            stopwatch.Restart();
            for (var i = 0; i < interations; i++)
            {
                foo.Equals(bar);
            }

            var equalsMilliseconds = stopwatch.ElapsedMilliseconds;
            output.WriteLine($"equals: {equalsMilliseconds} ms");

            equalsMilliseconds.Should().BeLessThan(
                reflectionEqualsMilliSeconds / 10);
        }
    }

    public static class ReflectionEqualsHelper<T>
    {
        private static readonly PropertyInfo[] properties = typeof(T).GetProperties();

        public static bool ReflectionEquals(T one, T other) =>
            properties.All(prop =>
                Equals(prop.GetValue(one), prop.GetValue(other)));
    }

    public class EmptyValueObject : ValueObject<EmptyValueObject>
    {
    }

    public class ValueObjectWithAPrimitive : ValueObject<ValueObjectWithAPrimitive>
    {
        public ValueObjectWithAPrimitive(string value1)
        {
            Value1 = value1 ?? throw new ArgumentNullException(nameof(value1));
        }

        public string Value1 { get; }
    }

    public class ValueObjectWithManyPrimitives : ValueObject<ValueObjectWithManyPrimitives>
    {
        public ValueObjectWithManyPrimitives(
            string value1,
            string value2,
            string value3,
            string value4,
            string value5,
            string value6,
            string value7)
        {
            Value1 = value1 ?? throw new ArgumentNullException(nameof(value1));
            Value2 = value2 ?? throw new ArgumentNullException(nameof(value2));
            Value3 = value3 ?? throw new ArgumentNullException(nameof(value3));
            Value4 = value4 ?? throw new ArgumentNullException(nameof(value4));
            Value5 = value5 ?? throw new ArgumentNullException(nameof(value5));
            Value6 = value6 ?? throw new ArgumentNullException(nameof(value6));
            Value7 = value7 ?? throw new ArgumentNullException(nameof(value7));
        }

        public string Value1 { get; }
        public string Value2 { get; }
        public string Value3 { get; }
        public string Value4 { get; }
        public string Value5 { get; }
        public string Value6 { get; }
        public string Value7 { get; }
    }
}
