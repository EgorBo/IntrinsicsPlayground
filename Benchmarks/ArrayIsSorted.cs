using System;
using System.Linq;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayIsSorted : ArrayBenchmarkBase
    {
        [Benchmark]
        public bool IsSorted_Simple()
        {
            return IsSorted_Simple(ArrayOfInts);
        }

        [Benchmark]
        public bool IsSorted_Simple_Optimized()
        {
            return IsSorted_Simple2(ArrayOfInts);
        }

        [Benchmark]
        public bool IsSorted_Sse41()
        {
            return ArrayIntrinsics.IsSorted_Sse41(ArrayOfInts);
        }

        [Benchmark(Baseline = true)]
        public bool IsSorted_Avx2()
        {
            return ArrayIntrinsics.IsSorted_Avx2(ArrayOfInts);
        }

        //[Benchmark]
        public bool IsSorted_LINQ()
        {
            // I am just kidding.. :)
            return ArrayOfInts.OrderBy(i => i).SequenceEqual(ArrayOfInts);
        }

		/*
        [Benchmark]
        public bool IsSorted_CppPinvoke()
        {
            return is_sorted_avx2_generic(ArrayOfInts, ArrayOfInts.Length);
        }

        [DllImport("NativeLib", CallingConvention = CallingConvention.Cdecl)]
        static extern bool is_sorted_avx2_generic(int[] array, int count);
		*/

        // simple implementations to benchmark against intrinsics:

        public static bool IsSorted_Simple(int[] array)
        {
            if (array.Length < 2)
                return true;

            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }

        public static bool IsSorted_Simple2(int[] array)
        {
            if (array.Length < 2)
                return true;

            // optimized by @consoleapp https://twitter.com/consoleapp/status/991380745234067458
            var current = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                var next = array[i];
                if (current > next)
                    return false;
                current = next;
            }
            return true;
        }
    }
}
