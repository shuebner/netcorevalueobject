using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;

namespace ValueObject.Test
{
    public static class Performance
    {
        public static IReadOnlyDictionary<string, long> Test(int iterations, ITestOutputHelper output, params (string name, Action action)[] runs)
        {
            var results = new Dictionary<string, long>(runs.Length);
            var stopwatch = new Stopwatch();

            for (int runCount = 0; runCount < runs.Length; runCount++)
            {
                var (name, action) = runs[runCount];
                stopwatch.Restart();
                for (var i = 0; i < iterations; i++)
                {
                    action();
                }
                var duration = stopwatch.ElapsedMilliseconds;

                results[name] = duration;
                output.WriteLine($"{name}: {duration} ms");
            }

            return results;
        }
    }
}
