using System;
using System.Globalization;

namespace KindOf
{
    public static class StringCanonicalizers
    {
        public static Func<string, string> Trim => str => str.Trim();

        public static Func<string, string> LowerCase(CultureInfo culture) => str => str.ToLower(culture);
    }
}
