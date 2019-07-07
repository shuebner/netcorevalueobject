using FluentAssertions;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace ValueObject.Test
{
    public class ValueObjectPerformanceTests
    {
        private readonly ITestOutputHelper output;

        public ValueObjectPerformanceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Equals_is_an_order_of_magnitude_faster_than_reflection()
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

    internal static class ReflectionEqualsHelper<T>
    {
        private static readonly PropertyInfo[] properties = typeof(T).GetProperties();

        public static bool ReflectionEquals(T one, T other) =>
            properties.All(prop =>
                Equals(prop.GetValue(one), prop.GetValue(other)));
    }
}
