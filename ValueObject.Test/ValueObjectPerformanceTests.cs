using FluentAssertions;
using ImmutableObject;
using System;
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

            var results = Performance.Test(1_000_000, output,
                ("reflection", () => ReflectionEqualsHelper<MultiPropertyFoo>.ReflectionEquals(foo, bar)),
                ("equals", () => foo.Equals(bar)));

            results["equals"].Should().BeLessThan(
                results["reflection"] / 10);
        }

        [Fact]
        public void Equals_is_faster_than_boxing_primitives_and_calling_equals()
        {
            var valueObject1 = new SomeValueObject(1);
            var valueObject2 = new SomeValueObject(2);

            var results = Performance.Test(10_000_000, output,
                ("boxing", () => valueObject1.BoxingEquals(valueObject2)),
                ("direct", () => valueObject1.DirectEquals(valueObject2)),
                ("equals", () => valueObject1.ValueEquals(valueObject2)));

            results["equals"].Should().BeLessThan((long)(results["boxing"] / 2));
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
