using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using IntrinsicsPlayground.Benchmarks.Misc;
using IntrinsicsPlayground.Tests;

namespace IntrinsicsPlayground
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            if (!Sse41.IsSupported || !Avx2.IsSupported)
                throw new NotSupportedException(":(");

            // Intrinsics:
            //BenchmarkRunner.Run<ArrayEqual>();
            //BenchmarkRunner.Run<ArrayIsSorted>();
            //BenchmarkRunner.Run<ArrayMax>();
            //BenchmarkRunner.Run<ArrayReverse>();
            //BenchmarkRunner.Run<ArraySum>();
            //BenchmarkRunner.Run<MatrixSum>();
            //BenchmarkRunner.Run<MatrixNegate>();

            // Sorting:
            //BenchmarkRunner.Run<SortingAlreadySortedArray>();
            //BenchmarkRunner.Run<SortingAlreadySortedButReversedArray>();
            //BenchmarkRunner.Run<SortingRandomArray>();
            BenchmarkRunner.Run<SortingNearlySortedArray>();
        }
    }
}
