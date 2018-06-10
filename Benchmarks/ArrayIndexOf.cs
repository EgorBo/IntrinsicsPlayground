using System;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayIndexOf : ArrayBenchmarkBase
    {
        [Benchmark]
        public int IndexOf_BCL()
        {
            return Array.IndexOf(ArrayOfInts, ArrayOfInts[ArrayOfInts.Length / 2]);
        }


        [Benchmark(Baseline = true)]
        public int IndexOf_Avx2()
        {
            return ArrayIntrinsics.IndexOf_Avx2(ArrayOfInts, ArrayOfInts[ArrayOfInts.Length / 2]);
        }
    }
}
