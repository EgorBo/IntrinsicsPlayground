using System;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;

namespace IntrinsicsPlayground
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            if (!Sse41.IsSupported || !Avx2.IsSupported)
                throw new NotSupportedException(":(");

            if (Environment.GetEnvironmentVariable("COMPlus_TieredCompilation") != "0")
                throw new Exception("Make sure Tiered JIT is disabled (enabled by default in .net core 3.0)");

            BenchmarkRunner.Run<ArrayIndexOf>();

            // Sorting:
            //BenchmarkRunner.Run<SortingAlreadySortedArray>();
            //BenchmarkRunner.Run<SortingAlreadySortedButReversedArray>();
            //BenchmarkRunner.Run<SortingRandomArray>();
            //BenchmarkRunner.Run<SortingNearlySortedArray>();
        }
    }
}
