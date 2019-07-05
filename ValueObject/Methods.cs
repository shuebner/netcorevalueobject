using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ValueObject
{
    public static class Methods
    {
        public static MethodInfo SequenceEqual(Type type) => typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.Name == nameof(Enumerable.SequenceEqual))
                .Where(method => method.GetParameters() is ParameterInfo[] parameters &&
                    parameters.Count() == 2 &&
                    parameters[0].ParameterType.IsGenericType &&
                    parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                    parameters[1].ParameterType.IsGenericType &&
                    parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Single()
                .MakeGenericMethod(type);
    }
}
