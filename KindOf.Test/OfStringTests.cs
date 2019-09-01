using FluentAssertions;
using System.Linq;
using Xunit;

namespace KindOf
{
    public class OfStringTests
    {
        [Fact]
        public void Can_be_sorted()
        {
            var name1 = new FirstName("Adam");
            var name2 = new FirstName("Norman");
            var name3 = new FirstName("Zach");
            FirstName[] unorderedNames = new[] { name2, name1, name3 };

            FirstName[] orderedNames = unorderedNames.OrderBy(name => name).ToArray();

            orderedNames.Should().BeEquivalentTo(name1, name2, name3);
        }

        [Fact]
        public void Can_be_concatenated()
        {
            var name = new FirstName("Adam");

            string doubleName = name + name;

            doubleName.Should().Be("AdamAdam");
        }

        [Fact]
        public void Can_be_used_in_formattable_string()
        {
            var name = new FirstName("Adam");

            var formattable = $"foo {name} bar";

            formattable.Should().Be("foo Adam bar");
        }

        public class FirstName : OfString<FirstName>
        {
            public FirstName(string value)
                : base(value, Validators.None<string>(), Canonicalizers.None<string>())
            {
            }
        }
    }
}
