using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace KindOf
{
    public class OfIntegerTests
    {
        [Fact]
        public void Can_be_sorted()
        {
            var someInt1 = new SomeInteger(1);
            var someInt2 = new SomeInteger(2);
            var someInt3 = new SomeInteger(3);

            var sortedList = new[] { someInt3, someInt1, someInt2 }.OrderBy(i => i);

            sortedList.Should().BeEquivalentTo(someInt1, someInt2, someInt3);
        }

        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(1, 1, 0)]
        [InlineData(2, 1, 1)]
        public void Can_be_compared(int first, int second, int expectedResult)
        {
            var firstInt = new SomeInteger(first);
            var secondInt = new SomeInteger(second);

            var result = firstInt.CompareTo(secondInt);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(OperatorExamples))]
        public void Can_be_used_with_operators(Func<SomeInteger, SomeInteger, bool> op, int first, int second, bool expectedResult)
        {
            var firstInt = new SomeInteger(first);
            var secondInt = new SomeInteger(second);

            var result = op(firstInt, secondInt);

            result.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> OperatorExamples = new Dictionary<Func<SomeInteger, SomeInteger, bool>, (int first, int second, bool expectedResult)[]>()
        {
            [(first, second) => first == second] = new[]
            {
                (1, 1, true),
                (1, 2, false)
            },
            [(first, second) => first != second] = new[]
            {
                (1, 1, false),
                (1, 2, true)
            },
            [(first, second) => first > second] = new[]
            {
                (1, 2, false),
                (1, 1, false),
                (2, 1, true)
            },
            [(first, second) => first < second] = new[]
            {
                (1, 2, true),
                (1, 1, false),
                (2, 1, false)
            },
            [(first, second) => first >= second] = new[]
            {
                (1, 2, false),
                (1, 1, true),
                (2, 1, true)
            },
            [(first, second) => first <= second] = new[]
            {
                (1, 2, true),
                (1, 1, true),
                (2, 1, false)
            }
        }.SelectMany(pair => pair.Value
            .Select(value => new object[] { pair.Key }
                .Concat(new object[] { value.first, value.second, value.expectedResult })
                .ToArray()));

        public class SomeInteger : OfInteger<SomeInteger>
        {
            public SomeInteger(int value)
                : base(value)
            {
            }
        }

        public class SomeOtherInteger : OfInteger<SomeOtherInteger>
        {
            public SomeOtherInteger(int value)
                : base(value)
            {
            }
        }
    }
}
