using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace ValueObject.Test
{
    public class FieldValueObjectTests
    {
        [Fact]
        public void Initializer_When_inheriting_type_is_not_ValueObject_type()
        {
            Action act = () => new ValueObjectWithWrongTypeParameter();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ObjectEquals_When_other_is_null_Then_returns_false()
        {
            var isEqual = ((object)new FieldFoo<string>("42")).Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_of_other_type_Then_returns_false()
        {
            var isEqual = ((object)new FieldFoo<string>("42")).Equals("42");

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void ObjectEquals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new FieldFoo<string>("42");
            var isEqual = ((object)valueObject).Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_valueobject_has_no_properties_and_other_is_different_reference_Then_returns_true()
        {
            var isEqual = new EmptyFoo().Equals(new EmptyFoo());

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_other_is_null_Then_returns_false()
        {
            var isEqual = new FieldFoo<string>("42").Equals(null);

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_other_is_same_reference_Then_returns_true()
        {
            object valueObject = new FieldFoo<string>("42");
            var isEqual = valueObject.Equals(valueObject);

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_primitive_value_is_different_Then_returns_false()
        {
            var isEqual = new FieldFoo<int>(1)
                .Equals(new FieldFoo<int>(2));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_primitive_value_is_equal_Then_returns_true()
        {
            var isEqual = new FieldFoo<int>(1)
                .Equals(new FieldFoo<int>(1));

            isEqual.Should().BeTrue();
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData(null, "foo")]
        [InlineData("foo", null)]
        public void Equals_When_string_value_is_different_Then_returns_false(string value1, string value2)
        {
            var isEqual = new FieldFoo<string>(value1)
                .Equals(new FieldFoo<string>(value2));

            isEqual.Should().BeFalse();
        }

        [Theory]
        [InlineData("foo")]
        [InlineData(null)]
        public void Equals_When_string_value_is_equal_Then_returns_true(string value)
        {
            var isEqual = new FieldFoo<string>(value)
                .Equals(new FieldFoo<string>(value));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_equatables_are_equal_Then_returns_true()
        {
            FieldFoo<SomeEquatable> fieldFoo = new FieldFoo<SomeEquatable>(new SomeEquatable("42"));
            var isEqual = fieldFoo
                .Equals(new FieldFoo<SomeEquatable>(new SomeEquatable("42")));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_equatables_are_not_equal_Then_returns_false()
        {
            var isEqual = new FieldFoo<SomeEquatable>(new SomeEquatable("42"))
                .Equals(new FieldFoo<SomeEquatable>(new SomeEquatable("23")));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_equatables_are_both_null_Then_returns_true()
        {
            var isEqual = new FieldFoo<SomeEquatable>(null)
                .Equals(new FieldFoo<SomeEquatable>(null));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_array_contains_same_primitives_in_same_order_Then_returns_true()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3)));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_array_contains_same_primitives_in_different_order_Then_returns_false()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 3, 2)));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_array_contains_different_primitives_Then_returns_false()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 4)));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_same_order_Then_returns_true()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3)));

            isEqual.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_list_contains_same_primitives_in_different_order_Then_returns_false()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 3, 2)));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_list_contains_different_primitives_Then_returns_false()
        {
            var isEqual = new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 3))
                .Equals(new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create(1, 2, 4)));

            isEqual.Should().BeFalse();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(23, 23)]
        public void Equals_When_nullable_values_are_equal_Then_returns_true(int? oneValue, int? otherValue)
        {
            var isEqual = new FieldFoo<int?>(oneValue)
                .Equals(new FieldFoo<int?>(otherValue));

            isEqual.Should().BeTrue();
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(1, null)]
        [InlineData(1, 2)]
        public void Equals_When_nullable_values_are_not_equal_Then_returns_false(int? oneValue, int? otherValue)
        {
            var isEqual = new FieldFoo<int?>(oneValue)
                .Equals(new FieldFoo<int?>(otherValue));

            isEqual.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_When_fields_are_equal_Then_produces_same_result()
        {
            var foo1 = new MultiFieldFoo("first", "second", "third", "fourth", "fifth", "sixth", "seventh");
            var foo2 = new MultiFieldFoo("first", "second", "third", "fourth", "fifth", "sixth", "seventh");

            var hashCode1 = foo1.GetHashCode();
            var hashCode2 = foo2.GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_When_fields_are_different_Then_produces_different_result_with_high_enough_probability()
        {
            var foo1 = new MultiFieldFoo("first", "second", "third", "fourth", "fifth", "sixth", "seventh");
            var foo2 = new MultiFieldFoo("first", "second", "second", "fourth", "fifth", "sixth", "seventh");

            var hashCode1 = foo1.GetHashCode();
            var hashCode2 = foo2.GetHashCode();

            hashCode1.Should().NotBe(hashCode2);
        }

        [Theory]
        [MemberData(nameof(HashCodeExamplesWithNull))]
        public void GetHashCode_When_a_field_is_null_Then_still_works(MultiFieldFoo foo1, MultiFieldFoo foo2, bool expectedHashCodeEquals)
        {
            var hashCode1 = foo1.GetHashCode();
            var hashCode2 = foo2.GetHashCode();

            hashCode1.Equals(hashCode2).Should().Be(expectedHashCodeEquals);
        }

        [Theory]
        [MemberData(nameof(HashCodeExamplesWithLists))]
        public void GetHashCode_respects_list_member_order(FieldFoo<ImmutableArray<int>> foo1, FieldFoo<ImmutableArray<int>> foo2, bool expectedHashCodeEquals)
        {
            var hashCode1 = foo1.GetHashCode();
            var hashCode2 = foo2.GetHashCode();

            hashCode1.Equals(hashCode2).Should().Be(expectedHashCodeEquals);
        }

        public static IEnumerable<object[]> HashCodeExamplesWithNull => new[]
        {
            new object[]
            {
                new MultiFieldFoo("first", null, "third", "fourth", "fifth", "sixth", "seventh"),
                new MultiFieldFoo("first", null, "third", "fourth", "fifth", "sixth", "seventh"),
                true
            },
            new object[]
            {
                new MultiFieldFoo("first", null, "third", "fourth", "fifth", "sixth", "seventh"),
                new MultiFieldFoo("first", "second", null, "fourth", "fifth", "sixth", "seventh"),
                false
            },
            new object[]
            {
                new MultiFieldFoo("first", null, "foo", "fourth", "fifth", "sixth", "seventh"),
                new MultiFieldFoo("first", "foo", null, "fourth", "fifth", "sixth", "seventh"),
                false
            }
        };

        public static IEnumerable<object[]> HashCodeExamplesWithLists => new[]
        {
            new object[]
            {
                new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create<int>(1, 2, 3)),
                new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create<int>(1, 2, 3)),
                true
            },
            new object[]
            {
                new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create<int>(1, 2, 3)),
                new FieldFoo<ImmutableArray<int>>(ImmutableArray.Create<int>(1, 3, 2)),
                false
            }
        };

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

        private class ValueObjectWithWrongTypeParameter : FieldValueObject<OtherFoo>
        {
        }

        private class OtherFoo : FieldValueObject<OtherFoo>
        {
        }
    }
}
