using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace ValueObject.Test
{
    [Trait("Category", "Performanance")]
    public class ValueObjectPerformanceTests
    {
        private readonly ITestOutputHelper output;

        public ValueObjectPerformanceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Equals_is_an_order_of_magnitude_faster_than_reflection()
        {
            var foo = new MultiPropertyFoo(
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo");
            var bar = new MultiPropertyFoo(
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "foo",
                "bar");

            // init
            ReflectionEqualsHelper<MultiPropertyFoo>
                .ReflectionEquals(foo, bar);
            const long interations = 1_000_000;

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < interations; i++)
            {
                ReflectionEqualsHelper<MultiPropertyFoo>
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

        [Fact]
        public void Equals_is_faster_than_boxing_primitives_and_calling_equals()
        {
            var valueObject1 = new SomeValueObject(1);
            var valueObject2 = new SomeValueObject(2);

            const long interations = 10_000_000;

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < interations; i++)
            {
                valueObject1.BoxingEquals(valueObject2);
            }

            var boxingEqualsMilliSeconds = stopwatch.ElapsedMilliseconds;
            output.WriteLine($"boxing: {boxingEqualsMilliSeconds} ms");

            stopwatch.Restart();
            for (var i = 0; i < interations; i++)
            {
                valueObject1.DirectEquals(valueObject2);
            }

            var directEqualsMilliSeconds = stopwatch.ElapsedMilliseconds;
            output.WriteLine($"direct: {directEqualsMilliSeconds} ms");

            stopwatch.Restart();
            for (var i = 0; i < interations; i++)
            {
                valueObject1.ValueEquals(valueObject2);
            }

            var equalsMilliseconds = stopwatch.ElapsedMilliseconds;
            output.WriteLine($"equals: {equalsMilliseconds} ms");

            equalsMilliseconds.Should().BeLessThan((long)(boxingEqualsMilliSeconds / 2));
        }

        private class SomeValueObject
        {
            private readonly Func<SomeValueObject, SomeValueObject, bool> DeepEquals =
                DeepValueEquals.FromProperties<SomeValueObject>();

            public SomeValueObject(int value)
            {
                Value = value;
            }

            public int Value { get; }

            public bool BoxingEquals(SomeValueObject other) =>
                ((object)Value).Equals((object)other.Value);

            public bool DirectEquals(SomeValueObject other) =>
                Value == other.Value;

            public bool ValueEquals(SomeValueObject other) =>
                DeepEquals(this, other);
        }
    }

    internal static class ReflectionEqualsHelper<T>
    {
        private static readonly PropertyInfo[] properties = typeof(T).GetProperties();

        public static bool ReflectionEquals(T one, T other) =>
            properties.All(prop =>
                Equals(prop.GetValue(one), prop.GetValue(other)));
    }
}
