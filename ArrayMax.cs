
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayMax
    {
        int[] testArray;

        [GlobalSetup]
        public void GlobalSetup()
        {
            const int count = 1024 * 32;
            var range = Enumerable.Range(0, count);
            testArray = range.Concat(range.Reverse()).ToArray();
        }

        [Benchmark]
        public int Max_LINQ()
        {
            // LINQ is so slow :(
            return testArray.Max();
        }

        [Benchmark(Baseline = true)]
        public int Max_Simple()
        {
            return Max_Simple(testArray);
        }

        [Benchmark]
        public int Max_AVX()
        {
            return Max_AVX(testArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max_Simple(int[] array)
        {
            int max = int.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (item > max)
                    max = item;
            }
            return max;
        }

        public static unsafe int Max_AVX(int[] array)
        {
            if (!Avx2.IsSupported)
                return Max_Simple(array);

            Vector256<int> max = Avx.SetAllVector256<int>(int.MinValue);
            fixed (int* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8) //16 for AVX512
                {
                    var current = Avx.LoadVector256(ptr + i);
                    max = Avx2.Max(current, max);
                }
            }

            // seems like there is no simple way to calculate horizontal maximum in __m256 
            // so let's just do simзle FOR for these 8 values
            var maxArray = new int[8];
            fixed (int* result = &maxArray[0])
                Avx.Store(result, max);

            return Max_Simple(maxArray);
        }
    }
}
