using FluentAssertions;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace ValueObject.Test
{
    public class WithTests
    {
        private static readonly DateTimeOffset SomeDateTime = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.Zero);
        private readonly ITestOutputHelper _output;

        public WithTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void With_returns_new_object_with_set_value()
        {
            var foo1 = new Foo(3, "second", SomeDateTime);

            var newFoo = foo1.With(foo => foo.Value2, "two");

            foo1.Value2.Should().Be("second");
            newFoo.Should().BeEquivalentTo(new
            {
                Value1 = 3,
                Value2 = "two",
                Value3 = SomeDateTime
            });
        }

        [Fact]
        [Trait("Category", "Performanance")]
        public void With_is_faster_than_reflection_implementation()
        {
            var foo = new Foo(42, "second", SomeDateTime);

            var results = Performance.Test(500_000, _output,
                ("explicit", () => foo.ExplicitWithValue2("two")),
                ("ctor", () => foo.ExplicitWithCtor(newValue2: "two")),
                ("reflection", () => foo.ReflectionWith(f => f.Value2, "two")),
                ("expression", () => foo.With(f => f.Value2, "two")));

            results["expression"].Should().BeLessThan((long)(results["reflection"] * .8));
        }

        private class Foo
        {
            private static readonly ConstructorInfo Ctor;
            private static readonly ParameterInfo[] CtorParamInfos;
            private static readonly ImmutableDictionary<string, FieldInfo> FieldByParamName;

            private static readonly Func<Foo, string, object, Foo> WithFunc;

            // eager initialize
            static Foo()
            {
                WithFunc = ImmutableObject.With.For<Foo>();

                // for reflection
                var thisType = typeof(Foo);
                Ctor = thisType.GetConstructors().Single();
                CtorParamInfos = Ctor.GetParameters();
                FieldByParamName = thisType
                    .GetFields()
                    .ToImmutableDictionary(field => ToParameterName(field.Name), field => field);
            }

            public readonly int Value1;
            public readonly string Value2;
            public readonly DateTimeOffset Value3;

            public Foo(int value1, string value2, DateTimeOffset value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }

            public Foo With<T>(Expression<Func<Foo, T>> fieldExpr, T value) =>
                WithFunc(this, ((MemberExpression)fieldExpr.Body).Member.Name, value);

            public Foo ExplicitWithValue2(string newValue2) =>
                new Foo(Value1, newValue2, Value3);

            public Foo ExplicitWithCtor(
                int? newValue1 = null,
                string newValue2 = null,
                DateTimeOffset? newValue3 = null) =>
                new Foo(
                    newValue1 ?? Value1,
                    newValue2 ?? Value2,
                    newValue3 ?? Value3);

            public Foo ReflectionWith<T>(Expression<Func<Foo, T>> fieldExpr, T value)
            {
                string propertyName = ((MemberExpression)fieldExpr.Body).Member.Name;
                var ctorParameterName = ToParameterName(propertyName);
                var ctorParamValues = CtorParamInfos.Select(param => param.Name == ctorParameterName
                    ? value
                    : FieldByParamName[param.Name].GetValue(this)).ToArray();

                return (Foo)Ctor.Invoke(ctorParamValues);
            }

            private static string ToParameterName(string memberName) =>
                string.Concat(char.ToLowerInvariant(memberName[0]), memberName.Substring(1));
        }
    }
}
