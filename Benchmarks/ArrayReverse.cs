using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayReverse : BenchmarkBase
    {
        [Benchmark]
        public int[] Reverse_BCL()
        {
            var copy = ArrayOfInts.ToArray();
            Array.Reverse(copy, 0, copy.Length); // https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/src/System/Array.cs#L1568-L1592
            return copy;
        }

        [Benchmark(Baseline = true)]
        public int[] Reverse_SSE()
        {
            var copy = ArrayOfInts.ToArray();
            ArrayIntrinsics.Reverse_Sse2(copy);
            return copy;
        }
    }
}
