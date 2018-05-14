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

            //BenchmarkRunner.Run<ArrayEqual>();
            //BenchmarkRunner.Run<ArrayIsSorted>();
            //BenchmarkRunner.Run<ArrayMax>();
            BenchmarkRunner.Run<ArrayReverse>();
            //BenchmarkRunner.Run<ArraySum>();
        }
    }
}
